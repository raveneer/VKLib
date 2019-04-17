using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VKLib.IO
{
    public static class FolderHelper
    {
        /// <summary>
        /// 폴더내의 모든 파일을 가져온다. 폴더 안의 폴더도 가져옴.
        /// </summary>
        public static IEnumerable<string> GetAllFileNames(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        yield return files[i];
                    }
                }
            }
        }
    }



    public static class FolderNameFixer
    {
        public static string Fix(string folderName)
        {
            //제목으로 쓸 수 없는게 하나라도 있으면, 해당 문자열들을 삭제하고 제목을 짓는다.
            var notUseableCharsForFolderName = new[] { '\\', ':', '*', '?', '"', '<', '>', '|' };
            if (folderName.Any(c => notUseableCharsForFolderName.Contains(c)))
            {
                Console.WriteLine($"!{folderName} 은 파일명으로 사용할 수 없는 문자열을 가지고 있습니다.");
                folderName = new string(folderName.Select(c => notUseableCharsForFolderName.Contains(c) ? ' ' : c).ToArray());
                Console.WriteLine("!해당 문자열을 빈칸으로 바꾸었습니다!");
            }
            //너무 길때는 자른다.
            if (folderName.Length > 100)
            {
                folderName.Remove(100);
                Console.WriteLine("!파일명이 너무 깁니다. 자릅니다!");
            }

            return folderName;
        }

    }

    
}
