/*
 * MIT License
 *
 * Copyright (c) 2022-2023 Aptivi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 */

using Extensification.StringExts;
using System;
using TermRead.Reader;
using TermRead.Tools;

namespace TermRead.Bindings.BaseBindings
{
    internal class Yank : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } = 
        { 
            new ConsoleKeyInfo('\u0019', ConsoleKey.Y, false, false, true)
        };

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // Re-write the text and set the current cursor position as appropriate
            string toYank = state.KillBuffer.ToString();
            state.CurrentText.Insert(state.CurrentTextPos, toYank);
            string renderedText = state.PasswordMode ? TermReaderSettings.PasswordMaskChar.ToString().Repeat(state.currentText.ToString().Length) : state.currentText.ToString();
            ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            ConsoleWrapperTools.ActionWriteString(renderedText);
            PositioningTools.GoForward(toYank.Length, ref state);
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
