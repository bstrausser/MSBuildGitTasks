﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NGit;
using NGit.Api;
using NGit.Revwalk;
using Sharpen;

namespace GitTasks
{
	public class Clone : Task
	{
		private string _SHA;

		/// <summary>
		/// Gets or sets the repository to clone.
		/// </summary>
		/// <value>
		/// The repository to clone.
		/// </value>
		[Required]
		public string RepositoryToClone { get; set; }

		/// <summary>
		/// Gets or sets the target directory.
		/// </summary>
		/// <value>
		/// The target directory.
		/// </value>
		[Required]
		public string TargetDirectory { get; set; }

		/// <summary>
		/// Gets the SHA of the latest commit in the given repository.
		/// </summary>
		[Output]
		public string SHA
		{
			get { return _SHA; }
		}

		/// <summary>
		/// Gets or sets the branch to switch to.
		/// </summary>
		/// <value>
		/// The branch to switch to.
		/// </value>
		public string BranchToSwitchTo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns>
		/// true if the task successfully executed; otherwise, false.
		/// </returns>
		public override bool Execute()
		{
			try
			{
				var git = new NGit();

				git.Clone(RepositoryToClone, TargetDirectory);
				Log.LogMessage(MessageImportance.Normal, string.Format("Cloning {0} to {1}", RepositoryToClone, TargetDirectory));
				
				if (!string.IsNullOrEmpty(BranchToSwitchTo) && BranchToSwitchTo.ToLower() != "master")
				{
					git.CheckoutBranch(TargetDirectory, BranchToSwitchTo);
					Log.LogMessage(MessageImportance.Normal, string.Format("Checking out branch/SHA '{0}'", BranchToSwitchTo));
				}

				_SHA = git.GetLatestSHA(TargetDirectory);
				Log.LogMessage(MessageImportance.Normal, string.Format("Latest commit is '{0}'", _SHA));
			}
			catch (Exception ex)
			{
				Log.LogErrorFromException(ex);
				return false;
			}

			return true;
		}

	}
}
