using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Discord_Token_Checker
{
    internal class Program
    {
        private static string valid = "";
        private static string invalid = "";

        private static void Main(string[] args)
        {
            List<string> tokens = File.ReadAllLines("Tokens.txt").Reverse().ToList();
            Console.WriteLine($"Loaded {tokens.Count} tokens!");
            Thread.Sleep(1500);
            Console.WriteLine("Checking...");
            Parallel.ForEach(new List<string>(tokens), new ParallelOptions() { MaxDegreeOfParallelism = 2 }, token =>
            {
                try
                {
                    Console.Title = $"Valids: {valid} | Invalid: {invalid} ";
                    DiscordClient client = new DiscordClient(token, new DiscordConfig() { GetFingerprint = false });
                    Thread.Sleep(1500);
                    Console.WriteLine($"[+] VALID: {client.Token} | User: {client.User}");
                    File.WriteAllText($"Valids.txt", $"{client.Token}");
                }
                catch (InvalidTokenException ex)
                {
                    Console.WriteLine($"Invalid: {ex.Token}");
                }
                catch (DiscordHttpException ex)
                {
                    Console.WriteLine(ex);
                }
                catch (AggregateException)
                {
                    Thread.Sleep(2500);
                }
            });
            Console.WriteLine("Finished, Press enter to exit");
            Console.ReadLine();
        }
    }
}
