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
    internal class LoAndForwardOneWord : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        {
            new ConsoleKeyInfo('v', ConsoleKey.V, false, true, false)
        };

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // If we're at the start of the text, bail.
            if (state.CurrentTextPos == state.CurrentText.Length)
                return;

            // Get the length of a word
            int steps = 0;
            state.CurrentText[state.CurrentTextPos] = char.ToLower(state.CurrentText[state.CurrentTextPos]);
            for (int i = state.CurrentTextPos; i < state.CurrentText.Length; i++) 
            {
                char currentChar = state.CurrentText[i];
                if (char.IsWhiteSpace(currentChar))
                    steps++;
                if (!char.IsWhiteSpace(currentChar))
                {
                    steps++;
                    if (i == state.CurrentText.Length - 1 || char.IsWhiteSpace(state.CurrentText[i + 1]))
                        break;
                }
            }
            string renderedText = state.PasswordMode ? TermReaderSettings.PasswordMaskChar.ToString().Repeat(state.currentText.ToString().Length) : state.currentText.ToString();
            ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            ConsoleWrapperTools.ActionWriteString(renderedText);
            PositioningTools.GoForward(steps, ref state);
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
