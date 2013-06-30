using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System.Net;
using System;
using System.Threading.Tasks;
using PreyCore;

namespace PreyBackgroundAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            // Prevents notifyComplete not being called because the request took too long
            var t = TaskEx.Delay(TimeSpan.FromSeconds(25));
            t.ContinueWith((o) => {
                System.Diagnostics.Debug.WriteLine("Forced Notify");
                NotifyComplete(); 
            });

            // Try to get the missing signal and wait
            Task<bool> isMissingTask = PreyServices.checkIfMissing();
            isMissingTask.Wait();

            if (isMissingTask.IsCompleted)
            {
                if (isMissingTask.Result)
                {
                    // Check if the expiration time is aproaching
                    DateTime now = DateTime.Now;
                    #if(DEBUG)
                        task.ExpirationTime = now.AddMinutes(35); /* change the expiration time for testing */
                    #endif

                    //remaining time to the expiration date
                    TimeSpan span = task.ExpirationTime.Subtract(now);

                    if (span < TimeSpan.FromDays(2))
                    {
                        // Warn to the user that the periodic task is going to expire
                        System.Diagnostics.Debug.WriteLine("ScheduleAgent, the periodic task is goint to expire");
                        WarnExpiration(span);
                    }
                }
                else 
                {
                    // Go crazy!!; the device is missing
                }

                NotifyComplete();
            }
            else if (isMissingTask.IsFaulted)
            {
                AggregateException exceptions = isMissingTask.Exception;
            }           

        }

        /// <summary>
        /// Show a toast notification to warn the user about the comming expiration of the background task
        /// If the close prey on notification tap setting is enabled taping the notification, will open the 
        /// application and close it after re-registering the periodic task
        /// </summary>
        /// <param name="span">remaining time to the expiration date</param>
        private void WarnExpiration(TimeSpan span)
        {
            string toastContent = "will expire {0} {1}. Please Tap...";
            if (span.Days > 0)
                toastContent = String.Format(toastContent, span.Days, (span.Hours == 1) ? "day" : "days");
            else if(span.Hours > 0)
                toastContent = String.Format(toastContent, span.Hours, (span.Hours == 1) ? "hour" : "hours");
            else
                toastContent = String.Format(toastContent, span.Minutes, (span.Minutes == 1) ? "minute" : "minutes");
            
            ShellToast toast = new ShellToast();
            toast.Title = "Prey";
            toast.Content = toastContent;
            toast.NavigationUri = new Uri("/Pages/SettingsPage.xaml?page=1", UriKind.Relative);
            toast.Show();
        }

        
    }
}