using System;

public class Program
{
    public static void Main()
    {       

        IDeviceFactory factory = new NcrFactory();

        IButton button = factory.CreateButton();

        IKeyboard keyboard = factory.CreateKeyboard();

        IScreen screen = factory.CreateScreen();
    }
}

public interface IButton
{
    void Draw();
}

public interface IKeyboard
{
    void Type();
}

public interface IScreen
{
    void Show();
}

public class NcrButton : IButton
{
    public void Draw()
    {
        Console.WriteLine("NCR Button");
    }
}

public class NcrKeyboard : IKeyboard
{
    public void Type()
    {
        Console.WriteLine("NCR Keyboard");
    }
}

public class NcrScreen : IScreen
{
    public void Show()
    {
        Console.WriteLine("NCR Screen");
    }
}

public class DieboldButton : IButton
{
    public void Draw()
    {
        Console.WriteLine("Diebold Button");
    }
}

public class DieboldKeyboard : IKeyboard
{
    public void Type()
    {
        Console.WriteLine("Diebold Keyboard");
    }
}

public class DieboldScreen : IScreen
{
    public void Show()
    {
        Console.WriteLine("Diebold Screen");
    }
}

public interface IDeviceFactory
{
    IButton CreateButton();

    IKeyboard CreateKeyboard();

    IScreen CreateScreen();
}

public class NcrFactory : IDeviceFactory
{
    public IButton CreateButton()
    {
        return new NcrButton();
    }

    public IKeyboard CreateKeyboard()
    {
        return new NcrKeyboard();
    }

    public IScreen CreateScreen()
    {
        return new NcrScreen();
    }
}

public class DieboldFactory : IDeviceFactory
{
    public IButton CreateButton()
    {
        return new DieboldButton();
    }

    public IKeyboard CreateKeyboard()
    {
        return new DieboldKeyboard();
    }

    public IScreen CreateScreen()
    {
        return new DieboldScreen();
    }
}