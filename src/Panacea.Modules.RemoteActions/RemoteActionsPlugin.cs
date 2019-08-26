using Panacea.Core;
using Panacea.Modularity;
using Panacea.Modularity.UiManager;
using Panacea.Modules.RemoteActions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Panacea.Modules.RemoteActions
{
    public class RemoteActionsPlugin : IPlugin
    {
        private PanaceaServices _core;

        public RemoteActionsPlugin(PanaceaServices core)
        {
            _core = core;
            SetupMessagesFromServerListener(_core);
            SetupRemoteSignInOutListener(_core);
        }
        private void SetupMessagesFromServerListener(PanaceaServices _core)
        {
            _core.WebSocket.On<MessageFromServer>("messageFromServer", HandleMessagesFromServer);
        }
        private void SetupRemoteSignInOutListener(PanaceaServices _core)
        {
            _core.WebSocket.On<SignoutMessage>("signout", (d) =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    try
                    {
                        if (d.User.Id == _core.UserService.User.Id && d.Terminal.Name == Environment.MachineName)
                        {
                            await _core.UserService.SetUser(null);
                            if (_core.TryGetUiManager(out IUiManager ui))
                            {
                                ui.GoHome();
                            }
                        }
                    }
                    catch
                    {
                        //ignore
                    }
                }));
            });
            _core.WebSocket.On<SignoutMessage>("signin", async (d) =>
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    try
                    {
                        if (d.User.Id != _core.UserService.User.Id && d.Terminal.Name == Environment.MachineName)
                            await _core.UserService.SetUser(d.User);
                    }
                    catch
                    {
                        //ignore
                    }
                }));
            });

            _core.WebSocket.On<SignoutMessage>("remoteSignOut", (d) =>
            {
                Application.Current.Dispatcher.Invoke(new Action(async () =>
                {
                    try
                    {
                        if (_core.TryGetUiManager(out IUiManager ui))
                        {
                            await ui.DoWhileBusy(async () =>
                            {
                                if (d.User.Id == _core.UserService.User.Id)
                                {
                                    await _core.UserService.LogoutAsync();// (true);
                                    ui.GoHome();
                                }
                            });
                        }
                    }
                    catch
                    {
                        //ignore
                    }
                }));
            });

            _core.WebSocket.On<SignoutMessage>("remoteSignIn", (d) =>
            {
                Application.Current.Dispatcher.Invoke(new Action(async () =>
                {
                    try
                    {
                        await _core.UserService.LoginAsync(d.User.DateOfBirth, d.User.Password);
                    }
                    catch
                    {
                        //ignore
                    }
                }));
            });
        }
        private void HandleMessagesFromServer(MessageFromServer msg)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(async () =>
                {
                    if (msg.Data != null)
                    {
                        if (msg.Data.Delay > 0)
                        {
                            await Task.Delay(msg.Data.Delay * 1000);
                        }
                        if (msg.Data.Logout)
                        {
                            await _core.UserService.LogoutAsync();
                        }
                    }
                    switch (msg.Action)
                    {
                        case "restartPanacea":
                            _core.WebSocket.Emit("offline", new { });
                            if (_core.TryGetUiManager(out IUiManager ui))
                            {
                                ui.Restart("restartPanacea command received");
                            }

                            break;
                        case "reboot":
                            //save a file to automatically login after reboot
                            Application.Current.Shutdown();
                            break;
                        case "showDevelopersPage":
                            //_core.GetUiManager().ShowDeveloperPage();
                            _core.Logger.Warn(this, "Not Implemented Yet");
                            break;
                        case "shutDownPanacea":
                            Application.Current.Shutdown();
                            break;
                    }
                });
            }
            catch
            {
            }
        }
        public Task BeginInit()
        {
            return Task.CompletedTask;
        }

        public Task EndInit()
        {
            return Task.CompletedTask;
        }

        public Task Shutdown()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            return;
        }
    }
}
