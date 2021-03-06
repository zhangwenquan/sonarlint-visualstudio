﻿/*
 * SonarLint for Visual Studio
 * Copyright (C) 2016-2020 SonarSource SA
 * mailto:info AT sonarsource DOT com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SonarLint.VisualStudio.Core.Analysis;
using SonarLint.VisualStudio.Integration.Vsix;

namespace SonarLint.VisualStudio.Integration.UnitTests
{
    public sealed class DummySonarLintDaemon : ISonarLintDaemon
    {
        #region Test helper methods

        public IEnumerable<AnalysisLanguage> SupportedLanguages { get; set; }

        public int StartCallCount { get; private set; }
        public int RequestAnalysisCallCount { get; private set; }

        /// <summary>
        /// Optional - code to execute when Start is called
        /// </summary>
        public Action StartOperation { get; set; }

        /// <summary>
        /// Optional - code execute when RequestAnalysis is called
        /// </summary>
        public Action RequestAnalysisOperation { get; set; }

        public void SimulateDaemonReady(EventArgs args)
        {
            this.IsRunning = true;
            this.Ready?.Invoke(this, args);
        }

        #endregion

        #region ISonarLintDaemon methods

        public bool IsRunning { get; set; } /* publicly settable for testing */

        public event EventHandler<EventArgs> Ready;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool IsAnalysisSupported(IEnumerable<AnalysisLanguage> languages)
        {
            return SupportedLanguages?.Intersect(languages).Any() ?? false;
        }

        public void ExecuteAnalysis(string path, string charset, IEnumerable<AnalysisLanguage> detectedLanguages,
            IIssueConsumer consumer, IAnalyzerOptions analyzerOptions, CancellationToken cancellationToken)
        {
            RequestAnalysisCallCount++;
            RequestAnalysisOperation?.Invoke();
        }

        public void Start()
        {
            StartCallCount++;
            StartOperation?.Invoke();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        #endregion ISonarLintDaemon methods
    }
}
