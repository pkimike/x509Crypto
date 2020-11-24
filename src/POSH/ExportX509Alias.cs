﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Org.X509Crypto;

namespace X509CryptoPOSH
{
    [Cmdlet(VerbsData.Export, nameof(X509Alias))]
    [OutputType(typeof(FileInfo))]
    public class ExportX509Alias : PSCmdlet
    {
        private string path = string.Empty;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The X509Alias to export")]
        [Alias(@"Alias", @"X509Alias")]
        public ContextedAlias Name { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "The path to the file to encrypt")]
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                if (System.IO.Path.IsPathRooted(value))
                {
                    path = value;
                }
                else
                {
                    path = new FileInfo(System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, value)).FullName;
                }
            }
        }

        [Parameter(HelpMessage = "If enabled and a file already exists in the indicated location for \"Path\" it will be overwritten. Default value is $False")]
        public bool Overwrite { get; set; } = false;

        [Parameter(HelpMessage = "If disabled, and a file already exists in the indicated location for \"Path\" it will be overwritten. Only applicable if \"Overwrite\" = $True. Default value is $True ")]
        public bool Confirm { get; set; } = true;

        private FileInfo Result;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            DoWork();
            WriteObject(Result);
        }

        private void DoWork()
        {
            if (File.Exists(Path) && (!(Overwrite && (!Confirm || Util.WarnConfirm($"A file already exists at the path {Path.InQuotes()}. Is it OK to overwrite it?", Constants.Affirm)))))
            {
                throw new X509CryptoException($"A file already exists at the path {Path.InQuotes()}. Set {nameof(Overwrite)} = {PoshSyntax.True} in order to enable overwriting.");
            }

            Name.Alias.Export(ref path, Overwrite);
            Util.ConsoleMessage($"{nameof(X509Alias)} aliasName was successfully exported to file {Path.InQuotes()}");
            Result = new FileInfo(Path);
        }
    }
}
