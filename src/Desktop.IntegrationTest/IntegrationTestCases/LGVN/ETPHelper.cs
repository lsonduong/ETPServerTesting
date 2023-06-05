using Energistics.Etp.Common.Datatypes;
using Energistics.Etp.Common;
using PDS.WITSMLstudio.Desktop.Core.Adapters;
using PDS.WITSMLstudio.Desktop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.LGVN
{
    public static class ETPHelper
    {

        public static IEtpExtender CreateEtpExtender(this IEtpSession session, IList<EtpProtocolItem> protocolItems)
        {
            return session.SupportedVersion == EtpVersion.v11
                ? new Etp11Extender(session, protocolItems)
                : new Etp12Extender(session, protocolItems) as IEtpExtender;
        }

        /// <summary>
        /// Opens a WebSocket connection and waits for the SocketOpened event to be called.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns>An awaitable task.</returns>
        public static async Task<bool> OpenAsync(this IEtpClient client)
        {
            var task = new Task<bool>(() => client.IsOpen);

            client.SocketOpened += (s, e) => task.Start();
            client.Open();

            return await task.WaitAsync();
        }

        /// <summary>
        /// Executes a task asynchronously and waits the specified timeout period for it to complete.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="task">The task to execute.</param>
        /// <param name="milliseconds">The timeout, in milliseconds.</param>
        /// <returns>An awaitable task.</returns>
        /// <exception cref="System.TimeoutException">The operation has timed out.</exception>
        public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, int milliseconds = 10000)
        {
            return await task.WaitAsync(TimeSpan.FromMilliseconds(milliseconds));
        }

        /// <summary>
        /// Executes a task asynchronously and waits the specified timeout period for it to complete.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="task">The task to execute.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>An awaitable task.</returns>
        /// <exception cref="System.TimeoutException">The operation has timed out.</exception>
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
