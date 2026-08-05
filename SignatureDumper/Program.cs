using System;
using System.Collections.Generic;
using System.IO;

namespace SignatureDumper
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (args.Length > 0 && (args[0] == "--help" || args[0] == "-h"))
            {
                PrintUsage();
                return 0;
            }

            string repoRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", ".."));

            string gameAssembliesDir = args.Length > 0
                ? args[0]
                : ResolveDefaultAssembliesDirectory();

            string outputDir = args.Length > 1
                ? args[1]
                : Path.Combine(repoRoot, "Decompiled");

            string[] targets = args.Length > 2
                ? args[2].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                : new[] { "Assembly-CSharp.dll", "Assembly-CSharp-firstpass.dll" };

            if (!Directory.Exists(gameAssembliesDir))
            {
                Console.Error.WriteLine($"[오류] Il2Cpp 시그니처 어셈블리 폴더를 찾을 수 없습니다: {gameAssembliesDir}");
                return 1;
            }

            Directory.CreateDirectory(outputDir);

            Console.WriteLine($"입력 폴더 : {gameAssembliesDir}");
            Console.WriteLine($"출력 폴더 : {outputDir}");
            Console.WriteLine();

            foreach (string targetName in targets)
            {
                string dllPath = Path.Combine(gameAssembliesDir, targetName.Trim());
                if (!File.Exists(dllPath))
                {
                    Console.WriteLine($"[건너뜀] 파일 없음: {dllPath}");
                    continue;
                }

                string moduleOutputDir = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(targetName.Trim()));
                if (Directory.Exists(moduleOutputDir))
                {
                    Directory.Delete(moduleOutputDir, recursive: true);
                }

                Console.WriteLine($"시그니처 덤프 중: {targetName} -> {moduleOutputDir}");

                try
                {
                    var options = new SignatureDumperOptions
                    {
                        TargetAssemblyPath = dllPath,
                        SearchDirectory = gameAssembliesDir,
                        OutputDirectory = moduleOutputDir
                    };

                    var dumper = new AssemblySignatureDumper(options);
                    dumper.Dump();

                    Console.WriteLine($"  완료: {targetName}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"  [실패] {targetName}: {ex.Message}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("모든 작업이 끝났습니다.");
            return 0;
        }

        static string ResolveDefaultAssembliesDirectory()
        {
            var candidates = new List<string>
            {
                Path.Combine("H:\\muse dash hwa", "MelonLoader", "Il2CppAssemblies"),
                Path.Combine("H:\\steam", "steamapps", "common", "MiSide", "MelonLoader", "Il2CppAssemblies"),
                Path.Combine("H:\\steam", "steamapps", "common", "Muse Dash", "MelonLoader", "Il2CppAssemblies")
            };

            foreach (string candidate in candidates)
            {
                if (Directory.Exists(candidate))
                {
                    return candidate;
                }
            }

            return Path.Combine("H:\\muse dash hwa", "MelonLoader", "Il2CppAssemblies");
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  SignatureDumper [inputDir] [outputDir] [target1,target2,...]");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  SignatureDumper");
            Console.WriteLine("  SignatureDumper H:\\muse dash hwa\\MelonLoader\\Il2CppAssemblies");
            Console.WriteLine("  SignatureDumper H:\\muse dash hwa\\MelonLoader\\Il2CppAssemblies H:\\out\\decompiled Assembly-CSharp.dll,Assembly-CSharp-firstpass.dll");
        }
    }
}
