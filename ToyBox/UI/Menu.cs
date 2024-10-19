using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using System.Collections.Generic;

public class Menu
{
    // Myra UI Desktop for rendering UI elements
    private Desktop _desktop;
    
    // Method to initialize the menu and components
    public void InitializeMenu()
    {
        // Create the desktop (root for Myra UI elements)
        _desktop = new Desktop();

        // Create a panel to hold the menu items
        var panel = new Panel
        {
            Width = 400,
            Height = 300
        };

        // Add a label
        var label = new Label
        {
            Text = "Component Selection Menu",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top
        };
        panel.Widgets.Add(label);

        // Add a button to test the display
        var button = new Button
        {
            Width = 200,
            Height = 50,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        // Add a label to the button's content
        var buttonLabel = new Label
        {
            Text = "Click Me",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        button.Content = buttonLabel;

        // Add an event to the button for testing
        button.Click += (sender, args) =>
        {
            label.Text = "Button Clicked!";
        };

        // Add the button to the panel
        panel.Widgets.Add(button);

        // Set the panel as the root widget of the desktop
        _desktop.Root = panel;
    }

    // Method to render the UI and any dragged component
    public void Draw()
    {
        // Render the Myra UI
        _desktop.Render();
    }
}
