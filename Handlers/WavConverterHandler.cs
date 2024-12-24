using JetBrains.Annotations;
using MediatR;
using WhisperAPI.Exceptions;
using WhisperAPI.Queries;
using WhisperAPI.Utils;
using Xabe.FFmpeg;

namespace WhisperAPI.Handlers;

[UsedImplicitly]
public sealed class WavConverterHandler(DirectoryInitializer directoryInitializer)
    : IRequestHandler<WavConvertQuery, (string, Func<Task>)>
{
    private const string WavExtension = ".wav";
    private const string Error = "Could not convert file to wav";

    public async Task<(string, Func<Task>)> Handle(WavConvertQuery request, CancellationToken token)
    {
        var audioFolder = directoryInitializer.AudioFilesFolder;
        var audioFolderExists = Directory.Exists(audioFolder);
        if (!audioFolderExists)
            Directory.CreateDirectory(audioFolder);

        var extension = request.Stream.ContentType[request.Stream.ContentType.IndexOf('/')..][1..];
        extension = extension.Insert(0, ".");

        var reqFile = Path.Combine(audioFolder, $"{Guid.NewGuid().ToString()[..4]}{extension}");
        var wavFile = Path.Combine(audioFolder, $"{Guid.NewGuid().ToString()[..4]}{WavExtension}");

        if (extension == WavExtension)
        {
            await using var reqFileTemp = File.Create(wavFile);
            await request.Stream.CopyToAsync(reqFileTemp, token);
        }
        else
        {
            await using (var reqFileTemp = File.Create(reqFile))
            {
                await request.Stream.CopyToAsync(reqFileTemp, token);
            }

            var mediaInfo = await FFmpeg.GetMediaInfo(reqFile, token);
            var audioStream = mediaInfo.AudioStreams.FirstOrDefault();
            if (audioStream is null)
                throw new FileProcessingException(Error);
            audioStream.SetCodec(AudioCodec.pcm_s16le);
            audioStream.SetChannels(1);

            var conversion = FFmpeg.Conversions.New()
                .AddStream(audioStream)
                .AddParameter("-ar 16000")
                .SetOutput(wavFile);
            try
            {
                await conversion.Start(token);
            }
            catch (Exception)
            {
                throw new FileProcessingException(Error);
            }    
        }

        Task Task()
        {
            if (File.Exists(reqFile))
            {
                File.Delete(reqFile);
            }
            File.Delete(wavFile);
            return System.Threading.Tasks.Task.CompletedTask;
        }

        return (wavFile, Task);
    }
}