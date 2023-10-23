using Avro.Specific;
using Energistics.Etp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HDS.iETP.IntegrationTestCases.LGVN.Helper
{
    public static class AsyncHelper
    {
        public static async Task<ProtocolEventArgs<T, TContext>> HandleAsync<T, TContext>(
        Action<ProtocolEventHandler<T, TContext>> action, int milliseconds)
        where T : ISpecificRecord
        {
            ProtocolEventArgs<T, TContext> args = null;
            var task = new Task<ProtocolEventArgs<T, TContext>>(() => args);

            action((s, e) =>
            {
                args = e;

                if (task.Status == TaskStatus.Created)
                    task.Start();
            });

            return await task.WaitAsync(milliseconds);
        }

        public static async Task<ProtocolEventArgs<T>> HandleAsync<T>(Action<ProtocolEventHandler<T>> action, int milliseconds)
            where T : ISpecificRecord
        {
            ProtocolEventArgs<T> args = null;
            var task = new Task<ProtocolEventArgs<T>>(() => args);

            action((s, e) =>
            {
                args = e;

                if (task.Status == TaskStatus.Created)
                    task.Start();
            });
            return await task.WaitAsync(milliseconds);
        }

        public static async Task<bool> OpenAsync(this IEtpClient client)
        {
            var task = new Task<bool>(() => client.IsOpen);

            client.SocketOpened += (s, e) => task.Start();
            client.Open();

            return await task.WaitAsync();
        }

        public static async Task<ProtocolEventArgs<T>> HandleAsync<T>(Action<ProtocolEventHandler<T>> action)
            where T : ISpecificRecord
        {
            ProtocolEventArgs<T> args = null;
            var task = new Task<ProtocolEventArgs<T>>(() => args);

            action((s, e) =>
            {
                args = e;

                if (task.Status == TaskStatus.Created)
                    task.Start();
            });
            return await task;
        }

        public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, int milliseconds = 120000)
        {
            return await task.WaitAsync(TimeSpan.FromMilliseconds(milliseconds));
        }

        public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            var tokenSource = new CancellationTokenSource();

            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, tokenSource.Token));
            if (completedTask == task)
            {
                tokenSource.Cancel();
                return await task;
            }

            throw new TimeoutException($"The operation has timed out.[{timeout}]");
        }
    }
}
