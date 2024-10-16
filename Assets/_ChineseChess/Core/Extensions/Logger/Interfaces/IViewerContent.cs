using System;

namespace ChuuniExtension.Loggers
{
    public interface IViewerContent
    {
        event Action<string, string> UpdateViewer;
    }
}