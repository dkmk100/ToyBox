using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using System.Collections.Generic;
using Myra.Graphics2D.UI.Styles;
using Myra.Graphics2D.TextureAtlases;

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

        // Create a panel to hold the menu items
        var panel = new Panel
        {
            Width = 200,
            Height = 700
        };

        // Add a label for the title
        var titleLabel = new Label
        {
            Text = "Component Selection Menu",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top,
            MinHeight = 20
        };
        panel.Widgets.Add(titleLabel);

        buttonPanel = panel;

        // Add a label to show the currently selected gate type
        selectedGateLabel = new Label
        {
            Text = "Selected Gate: None",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        panel.Widgets.Add(selectedGateLabel);

        // Set the panel as the root widget of the desktop
        _desktop.Root = panel;

        
    }

    public void RefreshButtons(string[] buttonNames)
    {
        foreach(var button in buttons)
        {
            buttonPanel.Widgets.Remove(button);
        }
        _currentY = 20;

        foreach(var name in buttonNames)
        {
            AddGateButton(buttonPanel, name, name);
        }
        selectedGateType = buttonNames[0];
    }

    // Helper method to add a gate selection button with a label
    private int _currentY = 20; // Initial Y position
    private int _verticalSpacing = 60; // Space between buttons (button height + some padding)

    private Button AddGateButton(Panel panel, string buttonText, string gateType)
    {
        var button = new Button
        {
            Width = 100,
            Height = 50,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Top = _currentY // Set the Y position for this button
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

        // Move the Y position down for the next button
        _currentY += _verticalSpacing;

        return button;
    }

    // Method to update the selected gate label and selected gate type
    public void SetSelectedGateType(string gateType)
    {
        selectedGateType = gateType;
        selectedGateLabel.Text = $"Selected Gate: {gateType}";
    }

    // Method to render the UI and any dragged component
    public void Draw()
    {
        // Render the Myra UI
        _desktop.Render();
    }
}
