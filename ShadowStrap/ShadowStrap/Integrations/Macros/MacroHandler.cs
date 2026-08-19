using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShadowStrap.Integrations.Macros
{
    public class MacroAction
    { 
        public string Key { get; set; } = "Space";
        public int DelayMs { get; set; } = 100;
    }

    public class Macro
    {
        public string Name { get; set; } = "New Macro";
        public Key ActivationKey { get; set; } = Key.F3;
        public List<MacroAction> Actions { get; set; } = new List<MacroAction>();
        public bool IsCycling { get; set; } = true;
    }

    public class MacroHandler
    {
        private bool _isRunning = false;

        public async Task ExecuteMacro(Macro macro)
        {
            _isRunning = true;
            while (_isRunning)
            {
                foreach (var action in macro.Actions)
                {
                    // Здесь будет вызов WinAPI для симуляции нажатия
                    Console.WriteLine($"Pressing {action.Key}");
                    await Task.Delay(action.DelayMs);
                }
                if (!macro.IsCycling) break;
            }
        }

        public void Stop() => _isRunning = false;
    }
}
