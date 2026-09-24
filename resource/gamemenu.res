// Made with XBLAH's Modding tool.
// Download it at https://xblah.dev/modding-tool/

"gamemenu"
{
	"0"
	{
		"label"	"#GameUI_GameMenu_ResumeGame"
		"HelpText"	""
		"command"	"ResumeGame"
		"ingameorder"	"0"
		"onlyingame"	"1"
	}
	"1"
	{
		"label"	"#GameUI_GameMenu_PlayerList"
		"HelpText"	""
		"command"	"OpenPlayerListDialog"
		"ingameorder"	"1"
		"onlyingame"	"1"
		"notsingle"	"1"
	}
	"2"
	{
		"label"	"#GameUI_GameMenu_SaveGame"
		"HelpText"	""
		"command"	"OpenSaveGameDialog"
		"ingameorder"	"2"
		"onlyingame"	"1"
		"notmulti"	"1"
	}
	"3"
	{
		"label"	"#GameUI_GameMenu_CreateServer"
		"HelpText"	"#GameUI_MainMenu_Hint_CreateServer"
		"command"	"OpenCreateMultiplayerGameDialog"
		"ingameorder"	"3"
		"notsingle"	"1"
	}
	"4"
	{
		"label"	"#GameUI_GameMenu_FindServers"
		"HelpText"	"#GameUI_MainMenu_Hint_FindServer"
		"command"	"OpenServerBrowser"
		"ingameorder"	"4"
		"notsingle"	"1"
	}
	"5"
	{
		"label"	"#GameUI_GameMenu_Options"
		"HelpText"	"#GameUI_MainMenu_Hint_Configuration"
		"command"	"OpenOptionsDialog"
		"ingameorder"	"5"
	}
	"6"
	{
		"label"	"#GameUI_GameMenu_ChangeGame"
		"HelpText"	"#GameUI_MainMenu_Hint_ChangeGame"
		"command"	"OpenChangeGameDialog"
		"ingameorder"	"6"
		"notsingle"	"1"
		"notmulti"	"1"
	}
	"7"
	{
		"label"	"#GameUI_GameMenu_LeaveGame"
		"HelpText"	""
		"command"	"Disconnect"
		"ingameorder"	"7"
		"onlyingame"	"1"
		"notmulti"	"1"
	}
	"8"
	{
		"label"	"#GameUI_GameMenu_Quit"
		"HelpText"	"#GameUI_MainMenu_Hint_QuitGame"
		"command"	"Quit"
		"ingameorder"	"8"
	}
}