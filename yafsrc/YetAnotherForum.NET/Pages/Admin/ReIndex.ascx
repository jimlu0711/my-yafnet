﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="YAF.Pages.Admin.ReIndex" Codebehind="ReIndex.ascx.cs" %>

<YAF:PageLinks runat="server" ID="PageLinks" />

<div class="row">
    <div class="col-xl-12">
        <div class="card mb-3">
            <div class="card-header">
                <YAF:IconHeader runat="server"
                                IconType="text-secondary"
                                IconName="database"
                                LocalizedTag="admin_reindex"
                                LocalizedPage="ADMINMENU"></YAF:IconHeader>
            </div>
            <div class="card-body">
                <asp:TextBox ID="txtIndexStatistics" runat="server"
                             Height="400px"
                             TextMode="MultiLine"
                             Visible="False"
                    CssClass="form-control"></asp:TextBox>
                <p class="card-text">
                        <YAF:LocalizedLabel ID="LocalizedLabel2" runat="server"
                                            LocalizedTag="SHOW_STATS"
                                            LocalizedPage="ADMIN_REINDEX" />
                    </p>
                    <p class="card-text">
                        <YAF:ThemeButton ID="GetStats"
                                         Type="Primary" runat="server"
                                         OnClick="GetStatsClick"
                                         Icon="database"
                                         TextLocalizedTag="TBLINDEXSTATS_BTN" />
                    </p>
                    <hr />
               <p class="card-text">
                        <YAF:ThemeButton ID="RecoveryMode" Type="Primary" runat="server" OnClick="RecoveryModeClick"
                                         ReturnConfirmTag="CONFIRM_RECOVERY"
                                         Icon="database" TextLocalizedTag="SETRECOVERY_BTN" />
                        <div class="form-check form-check-inline">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server"
                                                 RepeatLayout="UnorderedList"
                                                 CssClass="list-unstyled">
                            </asp:RadioButtonList>
                        </div>
                    </p>
                    <hr />
                <p class="card-text">
                        <YAF:LocalizedLabel ID="LocalizedLabel4" runat="server" LocalizedTag="REINDEX" LocalizedPage="ADMIN_REINDEX" />
                    </p>
                    <p class="card-text">
                        <YAF:ThemeButton ID="Reindex" Type="Primary" runat="server"
                                         OnClick="ReindexClick"
                                         ReturnConfirmTag="CONFIRM_REINDEX"
                                         Icon="database" TextLocalizedTag="REINDEXTBL_BTN" />
                    </p>
                    <hr />
                   <YAF:LocalizedLabel ID="LocalizedLabel3" runat="server" LocalizedTag="SHRINK" LocalizedPage="ADMIN_REINDEX" />
                <p class="card-text">
                        <YAF:ThemeButton ID="Shrink" runat="server"
                                         OnClick="ShrinkClick"
                                         Type="Primary"
                                         Icon="database"
                                         ReturnConfirmTag="CONFIRM_SHRINK"
                                         TextLocalizedTag="SHRINK_BTN" />
                    </p>
            </div>
        </div>
    </div>
</div>