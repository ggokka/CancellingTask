using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellingToken
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task<Int32> t = Task.Run(() => Sum(cts.Token, 10), cts.Token);
            cts.Cancel();

            try
            {
                Console.WriteLine("The last value is : " + t.Result);
            }
            catch (AggregateException x)
            {
                Console.WriteLine("Sum was canceled");
            }
            Console.ReadKey();
        }

        private static int Sum(CancellationToken ct, int n)
        {
            int sum = 0;
            for (; n > 0; n--)
            {

                sum += n;
                Console.WriteLine("The current sum is : " + sum);

                //작업 취소가 요청되면 OperationCanceledException을
                //InnerExceptions로 하는 AggregateException 발생
                ct.ThrowIfCancellationRequested();
            }

            return sum;
        }
    }
}