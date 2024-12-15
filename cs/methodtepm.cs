using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
namespace process
{
    public static partial class MethodClass
    {
        public static void tepm(ViewModel vm, object parameter)
        {
            MessageBox.Show("start");
            try
            {
                //process test
                var psinfo = new ProcessStartInfo();
                psinfo.FileName = vm.cmdstr;
                psinfo.WorkingDirectory = vm.cmdplacestr;
                psinfo.Arguments = vm.argsstr;
                psinfo.UseShellExecute = false;
                psinfo.RedirectStandardOutput = true;
                psinfo.RedirectStandardError = true;
                psinfo.CreateNoWindow = true;

                //子プロセスから受け取った標準出力をstring変数Bに格納
                using (var p = new Process())
                {
                    p.StartInfo = psinfo;
                    p.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            vm.resultstr += e.Data;
                        }
                    };
                    p.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            vm.resultstr += e.Data;
                        }
                    };
                    p.Start();
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    var task = Task.Run(() =>
                    {
                        p.WaitForExit();
                    });
                    while (!task.IsCompleted)
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
                    }
                    MessageBox.Show("end");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
