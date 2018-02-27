﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MiroBusDemoControl.ascx.cs" Inherits="ForMicroBus.UserControls.MiroBusDemoControl" %>

<asp:Label ID="Label1" runat="server"
    Text="Wait a while:" AssociatedControlID="WaitTime" />
<asp:LinkButton ID="WaitButton" runat="server" CssClass="btn btn-primary"
    OnClick="WaitButton_Click">Start Waiting</asp:LinkButton>
<div class="all-inline">
    <span>-1500ms min</span>
    <asp:TextBox ID="WaitTime" runat="server" Text="500"
        TextMode="Range" min="-1500" max="8000" step="500"
        ToolTip="Slide for time delay (in milliseconds)"/>
    <span>max 8000ms</span>
</div>
<asp:Label ID="SelectedWaitTime" runat="server" AssociatedControlID="WaitButton" />
<style>
    .all-inline {
        display: flex;
        align-items:center;
    }
    .all-inline > * {
        margin: 10px;
    }

    /* from http://danielstern.ca/range.css/#/ */
    input[type=range] {
        -webkit-appearance: none;
        width: 100%;
        margin: 13.8px 0;
    }

        input[type=range]:focus {
            outline: none;
        }

        input[type=range]::-webkit-slider-runnable-track {
            width: 100%;
            height: 8.4px;
            cursor: pointer;
            box-shadow: 1px 1px 1px #000000, 0px 0px 1px #0d0d0d;
            background: #3071a9;
            border-radius: 1.3px;
            border: 0.2px solid #010101;
        }

        input[type=range]::-webkit-slider-thumb {
            box-shadow: 1px 1px 1px #000000, 0px 0px 1px #0d0d0d;
            border: 1px solid #000000;
            height: 36px;
            width: 16px;
            border-radius: 3px;
            background: #ffffff;
            cursor: pointer;
            -webkit-appearance: none;
            margin-top: -14px;
        }

        input[type=range]:focus::-webkit-slider-runnable-track {
            background: #367ebd;
        }

        input[type=range]::-moz-range-track {
            width: 100%;
            height: 8.4px;
            cursor: pointer;
            box-shadow: 1px 1px 1px #000000, 0px 0px 1px #0d0d0d;
            background: #3071a9;
            border-radius: 1.3px;
            border: 0.2px solid #010101;
        }

        input[type=range]::-moz-range-thumb {
            box-shadow: 1px 1px 1px #000000, 0px 0px 1px #0d0d0d;
            border: 1px solid #000000;
            height: 36px;
            width: 16px;
            border-radius: 3px;
            background: #ffffff;
            cursor: pointer;
        }

        input[type=range]::-ms-track {
            width: 100%;
            height: 8.4px;
            cursor: pointer;
            background: transparent;
            border-color: transparent;
            color: transparent;
        }

        input[type=range]::-ms-fill-lower {
            background: #2a6495;
            border: 0.2px solid #010101;
            border-radius: 2.6px;
            box-shadow: 1px 1px 1px #000000, 0px 0px 1px #0d0d0d;
        }

        input[type=range]::-ms-fill-upper {
            background: #3071a9;
            border: 0.2px solid #010101;
            border-radius: 2.6px;
            box-shadow: 1px 1px 1px #000000, 0px 0px 1px #0d0d0d;
        }

        input[type=range]::-ms-thumb {
            box-shadow: 1px 1px 1px #000000, 0px 0px 1px #0d0d0d;
            border: 1px solid #000000;
            height: 36px;
            width: 16px;
            border-radius: 3px;
            background: #ffffff;
            cursor: pointer;
            height: 8.4px;
        }

        input[type=range]:focus::-ms-fill-lower {
            background: #3071a9;
        }

        input[type=range]:focus::-ms-fill-upper {
            background: #367ebd;
        }
</style>
