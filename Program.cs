using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DisneyInteractivePodFileExtractor {
    class Program {
        static void print(string value) {
            Console.WriteLine(value);
            System.Diagnostics.Debug.WriteLine(value);
        }

        static void Usage() {
            print("Usage: DisneyIneractivePodFileExtractor.exe \"path/to/file.pod\"");
            print("       The files will be extracted to \"path/to/_file.pod/\"");
        }

        static byte[] s_FileSignature = { 0x50, 0x6f, 0x64, 0x00, 0x66, 0x69, 0x6c, 0x65, 0x00, 0x00, 0x00, 0x00 }; // Pod\0file\0\0\0\0
        static byte[] s_FileSignature2 = { 0x50, 0x6f, 0x64, 0x20, 0x46, 0x69, 0x6c, 0x65, 0x00, 0x00, 0x00, 0x00 }; // Pod File\0\0\0\0

        static void Main(string[] args) {
            if(args.Length != 1) {
                Usage();
                return;
            }

            string filepath = args[0];
            string outdir = Path.Combine(Path.GetDirectoryName(filepath), '_' + Path.GetFileName(filepath));
            Directory.CreateDirectory(outdir);
            print("Extracting: \"" + filepath + "\" to \"" + outdir + "\"");

            using (BinaryReader b = new BinaryReader(File.Open(filepath, FileMode.Open))) {
                byte[] FileSignature = b.ReadBytes(12);
                for (int i = 0; i < FileSignature.Length; ++i) {
                    if (FileSignature[i] != s_FileSignature[i] && FileSignature[i] != s_FileSignature2[i]) {
                        print("FileSignature was not \"Pod\\0file\\0\\0\\0\\0\"!");
                        print("Are you sure this is a Disney Interactive Pod File?");
                        return;
                    }
                }
                
                int numFiles = b.ReadInt32();
                print("numFiles: " + numFiles.ToString());

                string[] fileNames = new string[numFiles];
                int[] fileSizes = new int[numFiles];
                for (int i = 0; i < numFiles; ++i) {
                    fileNames[i] = Encoding.ASCII.GetString(b.ReadBytes(12)).TrimEnd((Char)0);
                    fileSizes[i] = b.ReadInt32();
                    print("File name: \"" + fileNames[i] + "\" File size: " + fileSizes[i].ToString());
                }

                for(int i = 0; i < numFiles; ++i) {
                    using (BinaryWriter w = new BinaryWriter(File.Open(Path.Combine(outdir, fileNames[i]), FileMode.Create))) {
                        w.Write(b.ReadBytes(fileSizes[i]));
                    }
                }
                
                Debug.Assert(b.BaseStream.Position == b.BaseStream.Length);
            }
        }
    }
}
