﻿/*
 * Copyright (C) 2008, Google Inc.
 * Copyright (C) 2009, Gil Ran <gilrun@gmail.com>
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or
 * without modification, are permitted provided that the following
 * conditions are met:
 *
 * - Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above
 *   copyright notice, this list of conditions and the following
 *   disclaimer in the documentation and/or other materials provided
 *   with the distribution.
 *
 * - Neither the name of the Git Development Community nor the
 *   names of its contributors may be used to endorse or promote
 *   products derived from this software without specific prior
 *   written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Text;
using GitSharp.Util;

namespace GitSharp.Patch
{
	/// <summary>
	/// An error in a patch script.
	/// </summary>
	[Serializable]
	public class FormatError
	{
		/** Classification of an error. */

		#region Severity enum

		public enum Severity
		{
			/** The error is unexpected, but can be worked around. */
			WARNING,

			/** The error indicates the script is severely flawed. */
			ERROR
		}

		#endregion

		private readonly byte[] buf;
		private readonly String message;

		private readonly int offset;

		private readonly Severity severity;

		public FormatError(byte[] buffer, int ptr, Severity sev, String msg)
		{
			buf = buffer;
			offset = ptr;
			severity = sev;
			message = msg;
		}

		/** @return the severity of the error. */

		public Severity getSeverity()
		{
			return severity;
		}

		/** @return a message describing the error. */

		public string getMessage()
		{
			return message;
		}

		/** @return the byte buffer holding the patch script. */

		public byte[] getBuffer()
		{
			return buf;
		}

		/** @return byte offset within {@link #getBuffer()} where the error is */

		public int getOffset()
		{
			return offset;
		}

		/** @return line of the patch script the error appears on. */

		public string getLineText()
		{
			int eol = RawParseUtils.nextLF(buf, offset);
			return RawParseUtils.decode(Constants.CHARSET, buf, offset, eol);
		}

		public override string ToString()
		{
			var r = new StringBuilder();
			r.Append(Enum.GetName(typeof (Severity), getSeverity()));
			r.Append(": at offset ");
			r.Append(getOffset());
			r.Append(": ");
			r.Append(getMessage());
			r.Append("\n");
			r.Append("  in ");
			r.Append(getLineText());
			return r.ToString();
		}
	}
}