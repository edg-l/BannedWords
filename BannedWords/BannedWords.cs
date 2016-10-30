using System;
using System.IO;
using System.Linq;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace BannedWords
{
    [ApiVersion(1, 25)]
    public class BannedWords : TerrariaPlugin
    {
        #region Info
        public override string Name { get { return "BannedWords"; } }
        public override string Author { get { return "Ryozuki"; } }
        public override string Description { get { return "A plugin that censor words."; } }
        public override Version Version { get { return new Version(1, 0, 0); } }
        #endregion

        public ConfigFile config = new ConfigFile();

        public BannedWords(Main game) : base(game)
        {

        }

        #region Initialize
        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
            ServerApi.Hooks.ServerChat.Register(this, onChat);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
                ServerApi.Hooks.ServerChat.Deregister(this, onChat);
            }
        }

        void OnInitialize(EventArgs args)
        {
            LoadConfig();
        }

        private void onChat(ServerChatEventArgs args)
        {
            TSPlayer ply = TShock.Players[args.Who];

            if (ply == null)
                return;

            if (ply.HasPermission("bannedwords.bypass"))
                return;

            bool hasBannedWords = config.BannedWords.Any(s => args.Text.Contains(s));

            string text = args.Text;
            if (hasBannedWords)
            {
                foreach (string word in config.BannedWords)
                {
                    string replacement = new string(config.WordReplacement, word.Length);
                    text = text.Replace(word, replacement);
                }
                string text_with_name = ply.Name + ": " + text;
                TShock.Utils.Broadcast(text_with_name, new Color(ply.Group.R, ply.Group.G, ply.Group.B));
                args.Handled = true;
                return;
            }
            else
            {
                return;
            }
        }

        #region Load Config
        private void LoadConfig()
        {
            string path = Path.Combine(TShock.SavePath, "BannedWords.json");
            config = ConfigFile.Read(path);
        }
        #endregion
    }
}