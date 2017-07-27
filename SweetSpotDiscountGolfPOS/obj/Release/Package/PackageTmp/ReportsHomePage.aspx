<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsHomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsHomePage" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Reports">
            <div style="text-align: left">
        <asp:Label ID="lblReport" runat="server" Visible="false" Text="Report Access"></asp:Label>

                <asp:Label ID="lbldate" runat="server" Visible="false" Text="Select a date"></asp:Label>

        </div>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnRunReport">
            <h2>Reports Selection</h2>
            <hr />
            <%--Start Calendar--%>
            <div id="calStart"  runat="server">
                 <asp:TextBox ID="txtStartDate" ReadOnly="true" Width="195px" placeholder="Please select a starting date." Text="" runat="server"></asp:TextBox>
                <hr />
                <asp:Calendar ID="calStartDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="184px" Width="200px" OnSelectionChanged="calStart_SelectionChanged">
                    <DayHeaderStyle BackColor="#5FD367" Font-Bold="True" Font-Size="7pt" />
                    <NextPrevStyle VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#005555" BorderColor="Black" Font-Bold="True" />
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <WeekendDayStyle BackColor="#FFFFCC" />
                </asp:Calendar>
              
            </div>

            

            <hr />
            <%--End Calendar--%>
            <div id="calEnd"  runat="server" style="margin-top: -229px;"  class="auto-style1">
           <%-- <div id="calEnd" style="width:1210px;margin-top:auto;">--%>
                <asp:TextBox ID="txtEndDate" ReadOnly="true" Width="195px" placeholder="Please select a ending date." Text="" runat="server"></asp:TextBox>
                <hr />
             <div style ="width=30px; margin: 0 auto">
                <asp:Calendar ID="calEndDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="182px" Width="200px" OnSelectionChanged="calEnd_SelectionChanged">
                    <DayHeaderStyle BackColor="#5FD367" Font-Bold="True" Font-Size="7pt" />
                    <NextPrevStyle VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#808080" />
                    <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                    <SelectorStyle BackColor="#CCCCCC" />
                    <TitleStyle BackColor="#005555" BorderColor="Black" Font-Bold="True" />
                    <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <WeekendDayStyle BackColor="#FFFFCC" />
                </asp:Calendar>
            </div>
            </div>
            <hr />
            <asp:Button ID="btnRunReport" runat="server" Text="CashOut Report" Width="200px" OnClick="btnSubmit_Click" />
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            position: relative;
            left: 300px;
            top: -10px;
            width: 207px;
            height: 228px;
        }
        </style>
</asp:Content>

