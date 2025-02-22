using System.Windows.Controls;

namespace Essence
{
    internal static class Tabbie
    {
        public static T GetTemplateItem<T>(this Control elem, string name)
        {
            object obj = elem.Template.FindName(name, elem);
            if (obj is T)
            {
                return (T)obj;
            }
            return default(T);
        }
    }
}
