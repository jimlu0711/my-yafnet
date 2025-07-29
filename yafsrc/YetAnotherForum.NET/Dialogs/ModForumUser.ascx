﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModForumUser.ascx.cs" Inherits="YAF.Dialogs.ModForumUser" %>


<div class="modal fade" id="ModForumUserDialog" tabindex="-1" role="dialog" aria-labelledby="Moderate Forum User Dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">
                    <YAF:LocalizedLabel ID="LocalizedLabel1" runat="server" 
                                        LocalizedTag="TITLE"
                                        LocalizedPage="MOD_FORUMUSER"/>
                </h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                </button>
            </div>
            <div class="modal-body">
             <!-- Modal Content START !-->
                <YAF:LocalizedLabel ID="LocalizedLabel2" runat="server" LocalizedTag="USER" />
                <div class="input-group mb-3">
                    <asp:TextBox ID="UserName" runat="server"
                                 CssClass="form-control"/>
                    <asp:PlaceHolder runat="server" id="UserSelectHolder" Visible="False">
                        <select id="UserSelect" class="form-select"></select>
                    </asp:PlaceHolder>
                                
                    <asp:HiddenField id="SelectedUserID" runat="server" />
                </div>
                <div class="mb-3">
                    <asp:Label runat="server" AssociatedControlID="AccessMaskID">
                        <YAF:LocalizedLabel ID="LocalizedLabel3" runat="server" LocalizedTag="ACCESSMASK" />
                    </asp:Label>
                    <asp:DropDownList runat="server" ID="AccessMaskID" CssClass="select2-select" />
                </div>
            </div>
                        <!-- Modal Content END !-->           
            <div class="modal-footer">
                <YAF:ThemeButton runat="server" ID="Update"
                                 OnClick="UpdateClick"
                                 TextLocalizedTag="UPDATE"
                                 Type="Primary"
                                 Icon="save"/>
                <YAF:ThemeButton runat="server" ID="Cancel"
                                 DataDismiss="modal"
                                 TextLocalizedTag="CANCEL"
                                 Type="Secondary"
                                 Icon="times">
                </YAF:ThemeButton>
            </div>
        </div>
    </div>
</div>
