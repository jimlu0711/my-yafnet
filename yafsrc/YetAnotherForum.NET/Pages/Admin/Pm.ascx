﻿<%@ Control Language="c#" AutoEventWireup="True" Inherits="YAF.Pages.Admin.Pm" Codebehind="Pm.ascx.cs" %>


<YAF:PageLinks runat="server" ID="PageLinks" />

    <div class="row">
        <div class="col-xl-12">
            <div class="card mb-3">
                <div class="card-header">
                    <YAF:IconHeader runat="server"
                                    IconType="text-secondary"
                                    IconName="envelope-square"
                                    LocalizedTag="HEADER"
                                    LocalizedPage="ADMIN_PM"></YAF:IconHeader>
                </div>
                <div class="card-body">
                    <YAF:Alert runat="server"
                               Type="Info">
                        <YAF:Icon runat="server" IconName="info-circle" />
                        <YAF:LocalizedLabel ID="LocalizedLabel6" runat="server" LocalizedTag="PM_NUMBER" LocalizedPage="ADMIN_PM" />
                        <span class="badge text-bg-info"><asp:Label runat="server" ID="Count" /></span>
                    </YAF:Alert>
                    <div class="row">
                        <div class="mb-3 col-md-4">
                            <asp:Label runat="server"
                                       CssClass="form-label"
                                       AssociatedControlID="Days1">
                                <YAF:LocalizedLabel ID="LocalizedLabel5" runat="server"
                                                    LocalizedTag="DELETE_READ" LocalizedPage="ADMIN_PM" />
                            </asp:Label>
                            <div class="input-group">
                                <asp:TextBox runat="server" ID="Days1"
                                             CssClass="form-control form-control-days"
                                             TextMode="Number"
                                             data-min="1"  />
                                <div class="input-group-text">
                                    <YAF:LocalizedLabel runat="server"
                                                        LocalizedTag="DAYS"></YAF:LocalizedLabel>
                                </div>
                            </div>
                        </div>
                        <div class="mb-3 col-md-4">
                            <asp:Label runat="server"
                                       CssClass="form-label"
                                       AssociatedControlID="Days2">
                                <YAF:LocalizedLabel ID="LocalizedLabel4" runat="server"
                                                    LocalizedTag="DELETE_UNREAD" LocalizedPage="ADMIN_PM" />
                            </asp:Label>
                            <div class="input-group">
                                <asp:TextBox runat="server" ID="Days2"
                                             CssClass="form-control form-control-days"
                                             TextMode="Number"
                                             data-min="1" />
                                <div class="input-group-text">
                                    <YAF:LocalizedLabel runat="server"
                                                        LocalizedTag="DAYS"></YAF:LocalizedLabel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-footer text-center">
                    <YAF:ThemeButton ID="commit" Type="Primary" runat="server"
                        Icon="trash" TextLocalizedTag="DELETE" TextLocalizedPage="COMMON"
                        ReturnConfirmTag="CONFIRM_DELETE">
                    </YAF:ThemeButton>
                </div>
            </div>
        </div>
    </div>


