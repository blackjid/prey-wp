using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Scheduler;

namespace PreyClient.Utilities
{
    public static class PeriodicTaskHelper
    {
        static PeriodicTask periodicTask;
        static string periodicTaskName = "PreyPeriodicAgent";

        public static void StartPeriodicAgent()
        {
            // Obtain a reference to the period task, if one exists
            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;            

            // If the task already exists and background agents are enabled for the
            // application, you must remove the task and then add it again to update
            // the schedule
            if (periodicTask != null)
            {
                if (periodicTask.LastExitReason != AgentExitReason.Completed && periodicTask.LastExitReason != AgentExitReason.None)
                    System.Diagnostics.Debug.WriteLine("The background task failed to complete its last execution at {0:g} with an exit reason of {1}. ", periodicTask.LastScheduledTime, periodicTask.LastExitReason);

                //if (!periodicTask.IsEnabled)
                    //The background task was disabled by the user

                //if (periodicTask.ExpirationTime < DateTime.Now)
                    //The backgroundtask was expired
                    

                ScheduledActionService.Remove(periodicTaskName);
            }

            try
            {
                periodicTask = new PeriodicTask(periodicTaskName);
                periodicTask.Description = "This task check's whether you have marked the device as stolen or missing";
                ScheduledActionService.Add(periodicTask);

                // If debugging is enabled, use LaunchForTest to launch the agent in one minute.
                #if(DEBUG)
                    ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(10));
                #endif      
            }
            catch (InvalidOperationException ex)
            {
                //Unable to create or reschedule the background task
                periodicTask = null;

                //TODO: Warn the user that if the task is disabled, prey would no be able to get the info about the device 
                //unless the thief start the application, which is not likely
            }
            
                

        }

    }
}
