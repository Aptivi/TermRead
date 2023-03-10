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

namespace TermRead.Bindings
{
    /// <summary>
    /// Base key binding
    /// </summary>
    public abstract class BaseBinding : IBinding
    {
        /// <summary>
        /// Key to bind to
        /// </summary>
        public virtual ConsoleKeyInfo[] BoundKeys { get; }

        /// <summary>
        /// Does this binding cause the input to exit?
        /// </summary>
        public virtual bool IsExit { get; }

        /// <summary>
        /// Whether the binding matched
        /// </summary>
        /// <param name="input">Input key</param>
        public virtual bool BindMatched(ConsoleKeyInfo input)
        {
            bool match = false;
            foreach (var key in BoundKeys)
            {
                match = input.Key == key.Key &&
                        input.KeyChar == key.KeyChar &&
                        input.Modifiers == key.Modifiers;
                if (match)
                    break;
            }
            return match;
        }

        /// <summary>
        /// Do the action
        /// </summary>
        /// <param name="state">State of the reader</param>
        public virtual void DoAction(TermReaderState state)
        {
            // Insert the character, but in the condition that it's not a control character
            if (char.IsControl(state.pressedKey.KeyChar))
                return;
            state.CurrentText.Insert(state.CurrentTextPos, state.pressedKey.KeyChar);

            // Re-write the text and set the current cursor position as appropriate
            string renderedText = state.PasswordMode ? TermReaderSettings.PasswordMaskChar.ToString().Repeat(state.currentText.ToString().Length) : state.currentText.ToString();
            ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            ConsoleWrapperTools.ActionWriteString(renderedText);
            PositioningTools.GoForward(1, true, ref state);
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
