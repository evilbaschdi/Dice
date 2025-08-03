using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Dice.Avalonia.ViewModels;

namespace Dice.Avalonia;

/// <inheritdoc />
public class ViewLocator : IDataTemplate
{
    /// <inheritdoc />
    public Control Build(object data)
    {
        var name = data?.GetType().FullName!.Replace("ViewModel", "View");
        if (name == null)
        {
            return new TextBlock { Text = "View Not Found" };
        }

        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    /// <inheritdoc />
    public bool Match(object data) => data is ViewModelBase;
}