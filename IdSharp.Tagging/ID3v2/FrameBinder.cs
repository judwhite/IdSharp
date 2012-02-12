using System;
using System.ComponentModel;

namespace IdSharp.Tagging.ID3v2
{
    internal sealed class FrameBinder
    {
        private readonly FrameContainer m_FrameContainer;

        public FrameBinder(FrameContainer frameContainer)
        {
            m_FrameContainer = frameContainer;
        }

        public void Bind(INotifyPropertyChanged frame, string frameProperty, string tagProperty, Action validator)
        {
            /*bool found = false;
            foreach (System.Reflection.PropertyInfo pi in typeof(FrameContainer).GetProperties())
            {
                if (pi.Name == tagProperty)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
            }*/

            frame.PropertyChanged += delegate
            {
                m_FrameContainer.RaisePropertyChanged(tagProperty);
                if (validator != null) 
                    validator();
            };
        }
    }
}
