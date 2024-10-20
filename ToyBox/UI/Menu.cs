using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using System.Collections.Generic;
using Myra.Graphics2D.UI.Styles;
using Myra.Graphics2D.TextureAtlases;
using MonoGame.Extended;
using Myra.Graphics2D;

public class Menu
{
    // Myra UI Desktop for rendering UI elements
    private Desktop _desktop;
    
    // Variable to store the currently selected gate type
    private string selectedGateType;

    // Label to show the currently selected gate
    private Label selectedGateLabel;

    private List<Button> buttons = new List<Button>();

    private Panel buttonPanel;
    ScrollViewer buttonScroll;

    public string GetSelected()
    {
        return selectedGateType;
    }

    public bool IsOnUI()
    {
        return _desktop.IsMouseOverGUI;
    }

    // Method to initialize the menu and components
    public void InitializeMenu()
    {
        // Create the desktop (root for Myra UI elements)
        _desktop = new Desktop();

        var root = new Panel();

        // Create a panel to hold the menu items
        var panel = new Panel
        {
            Width = 100,
            Height = 1200
        };

        // Add a label for the title
        var titleLabel = new Label
        {
            Text = "Logic Gate Simulator",
            Scale = new Vector2(2f),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top,
            Top = 20
        };

        AddGateButton(root, "none", 20);
        AddGateButton(root, "new", 20+60*1);
        AddGateButton(root, "clear", 20 + 60 * 2);
        AddGateButton(root, "wire", 20 + 60 * 3);

        root.Widgets.Add(titleLabel);

        buttonPanel = panel;

        // Add a label to show the currently selected gate type
        selectedGateLabel = new Label
        {
            Text = "Selected Gate: None",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
            Scale = new Vector2(1.5f),
            Left = 100
        };

        root.Widgets.Add(selectedGateLabel);

        var scroll = new ScrollViewer
        {
            Width = 150,
            Top = buttonsStart
        };

        scroll.Content = panel;
        buttonScroll = scroll;

        root.Widgets.Add(scroll);

        // Set the panel as the root widget of the desktop
        _desktop.Root = root;

        
    }

    private int buttonsStart = 20 + 60*4;

    private int _verticalSpacing = 60; // Space between buttons (button height + some padding)

    public void RefreshButtons(string[] buttonNames)
    {
        foreach(var button in buttons)
        {
            buttonPanel.Widgets.Remove(button);
        }
        int y = 0;

        buttonPanel.Height = _verticalSpacing * (buttonNames.Length + 1);

        foreach(var name in buttonNames)
        {
            AddGateButton(buttonPanel, name, name, y);
            y += _verticalSpacing;
        }
        selectedGateType = buttonNames[0];
    }

    private Button AddGateButton(Panel panel, string name, int height)
    {
        return AddGateButton(panel, name, name, height);
    }

    private Button AddGateButton(Panel panel, string buttonText, string gateType, int height)
    {
        var button = new Button
        {
            Width = 100,
            Height = 50,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Top = height // Set the Y position for this button
        };

        // Create a label and set it as the button's content
        var label = new Label
        {
            Text = buttonText,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        button.Content = label;

        // Event handler for when the button is clicked
        button.Click += (sender, args) =>
        {
            // Call a method to handle setting the selected gate type
            SetSelectedGateType(gateType);
        };

        // Add the button to the panel
        panel.Widgets.Add(button);

        return button;
    }

    // Method to update the selected gate label and selected gate type
    public void SetSelectedGateType(string gateType)
    {
        selectedGateType = gateType;
        
    }

    // Method to render the UI and any dragged component
    public void Draw(int selected)
    {
        selectedGateLabel.Text = $"Selected Gate: {selectedGateType}, {selected}";
        buttonScroll.Height = _desktop.BoundsFetcher().Height - buttonsStart;
        // Render the Myra UI
        _desktop.Render();
    }
}
