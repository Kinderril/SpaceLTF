using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WindowLoseBattle : BaseWindow
{
    public void OnClickToMainMenu()
    {
        WindowManager.Instance.OpenWindow(MainState.start);
    }
}

