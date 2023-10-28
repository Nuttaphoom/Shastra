using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine; 

namespace Vanaring 
{
    public class EntityWindowManager 
    {
        private Stack<HierarchyUIWindow> _windowStack ;
        private CombatGraphicalHandler _graophicalHandler; 
        public EntityWindowManager()
        {
            _windowStack = new Stack<HierarchyUIWindow>(); 
        }

        public void SetNewEntity(CombatGraphicalHandler graophicalHandler)
        {
            _windowStack.Clear(); 

            _graophicalHandler = graophicalHandler;
        }

        public void PushInNewWindow(HierarchyUIWindow window)
        {
            if (_windowStack.Contains(window))
                return;
            
            if (_windowStack.Count > 0) 
                _windowStack.Peek().OnWindowOverlayed();

            _windowStack.Push(window) ;

            _windowStack.Peek().OnWindowDisplay(_graophicalHandler); 
        }

        public void PrevWindow()
        {
            if (_windowStack.Count > 0)
            {
                HierarchyUIWindow window = _windowStack.Pop();
                window.OnWindowOverlayed();

                window = _windowStack.Peek();

                window.OnWindowDisplay(_graophicalHandler);
            }
        }

        public void ClearStack()
        {
            if (_windowStack.Count > 0)
            {
                _windowStack.Pop().OnWindowOverlayed();

                while (_windowStack.Count > 0)
                {
                    _windowStack.Pop();
                }
            }
        }
    }
}
