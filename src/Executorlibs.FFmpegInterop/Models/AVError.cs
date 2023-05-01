namespace Executorlibs.FFmpegInterop.Models
{
    public enum AVError
    {
        Success = 0,

        Again = -11, // EAGAIN

        OutOfMemory = -12, // ENOMEM
        
        EndOfFile = -('E' | 'O' << 8 | 'F' << 16 | ' ' << 24), // AVERROR_EOF 

        InvalidData = -('I' | 'N' << 8 | 'D' << 16 | 'A' << 24) // AVERROR_INVALIDDATA
    }
}
