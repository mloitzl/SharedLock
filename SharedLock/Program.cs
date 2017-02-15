using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLock
{
    internal class Program
    {
        private const string SourceDir = "D:\\Docs\\Images";
        private const string TargetDir = "D:\\Docs\\Images\\Modified";

        private static void Main()
        {
            EnsureTargetDirectory();

            var files = Directory.GetFiles(SourceDir, "*.jpg");

            var sw = new Stopwatch();
            sw.Start();

            // Locked
            Parallel.ForEach(files, file =>
            {
                lock (SomeClass.Lock)
                {
                    var fileName = Path.GetFileName(file);
                    if (fileName == null) return;

                    var bitmap = new Bitmap(file);
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(Path.Combine(TargetDir, fileName));

                    Console.WriteLine("Processing {0} on thread {1}", fileName, Thread.CurrentThread.ManagedThreadId);
                }
            });

            Console.WriteLine($"Locked Parallel: {sw.ElapsedMilliseconds} ms");

            sw.Restart();

            Parallel.ForEach(files, file =>
            {
                var fileName = Path.GetFileName(file);
                if (fileName == null) return;

                var bitmap = new Bitmap(file);
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(TargetDir, fileName));

                Console.WriteLine("Processing {0} on thread {1}", fileName, Thread.CurrentThread.ManagedThreadId);
            });

            Console.WriteLine($"Real Parallel: {sw.ElapsedMilliseconds} ms");

            Console.ReadKey();
        }

        private static void EnsureTargetDirectory()
        {
            if (!Directory.Exists(TargetDir))
            {
                Directory.CreateDirectory(TargetDir);
            }
        }
    }
}