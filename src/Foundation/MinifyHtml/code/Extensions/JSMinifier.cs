using Sitecore.Foundation.MinifyHtml.Exceptions;
using System.IO;
using System.Text;

namespace Sitecore.Foundation.MinifyHtml.Extensions
{
    public class JsMinifier
    {
        private const int Eof = -1;

        private TextReader tr;
        private StringBuilder sb;
        private int theA;
        private int theB;
        private int theLookahead = Eof;

        private string Compress(string js)
        {
            using (this.tr = new StringReader(js))
            {
                this.sb = new StringBuilder();
                this.Jsmin();
                return this.sb.ToString(); // return the minified string  
            }
        }

        public static string MinifyJs(string js) //removed the out file path  
        {
            return new JsMinifier().Compress(js);
        }

        /// <summary>
        /// Copy the input to the output, deleting the characters which are
        /// insignificant to JavaScript.Comments will be removed.Tabs will be
        /// replaced with spaces.Carriage returns will be replaced with linefeeds.
        /// Most spaces and linefeeds will be removed.
        /// </summary>
        private void Jsmin()
        {
            this.theA = '\n';
            this.Action(3);
            while (this.theA != Eof)
            {
                switch (this.theA)
                {
                    case ' ':
                    {
                        this.Action(IsAlphanum(this.theB) ? 1 : 2);
                        break;
                    }
                    case '\n':
                    {
                        this.JsMinNewLine();
                        break;
                    }
                    default:
                    {
                        this.JsMinOtherCharacter();
                        break;
                    }
                }
            }
        }

        private void JsMinOtherCharacter()
        {
            switch (this.theB)
            {
                case ' ':
                {
                    if (IsAlphanum(this.theA))
                    {
                        this.Action(1);
                        break;
                    }

                    this.Action(3);
                    break;
                }
                case '\n':
                {
                    switch (this.theA)
                    {
                        case '}':
                        case ']':
                        case ')':
                        case '+':
                        case '-':
                        case '"':
                        case '\'':
                        {
                            this.Action(1);
                            break;
                        }
                        default:
                        {
                            this.Action(IsAlphanum(this.theA) ? 1 : 3);
                            break;
                        }
                    }

                    break;
                }
                default:
                {
                    this.Action(1);
                    break;
                }
            }
        }

        private void JsMinNewLine()
        {
            switch (this.theB)
            {
                case '{':
                case '[':
                case '(':
                case '+':
                case '-':
                {
                    this.Action(1);
                    break;
                }
                case ' ':
                {
                    this.Action(3);
                    break;
                }
                default:
                {
                    this.Action(IsAlphanum(this.theB) ? 1 : 2);
                    break;
                }
            }
        }

        /* action -- do something! What you do is determined by the argument: 
                1   Output A. Copy B to A. Get the next B. 
                2   Copy B to A. Get the next B. (Delete A). 
                3   Get the next B. (Delete B). 
           action treats a string as a single character. Wow! 
           action recognizes a regular expression if it is preceded by ( or , or =. 
        */
        private void Action(int d)
        {
            this.Action1(d);
            this.Action2(d);
            if (!this.Action3(d)) this.theB = this.Next();
        }

        private void Action1(int d)
        {
            if (d <= 1)
            {
                this.Put(this.theA);
            }
        }

        private void Action2(int d)
        {
            if (d > 2) return;

            this.theA = this.theB;
            if (this.theA != '\'' && this.theA != '"') return;

            while (true)
            {
                this.Put(this.theA);
                this.theA = this.Get();
                if (this.theA == this.theB)
                {
                    break;
                }

                if (this.theA <= '\n')
                {
                    throw new MinifierException($"Error: JSMIN unterminated string literal: {this.theA}\n");
                }

                if (this.theA != '\\') continue;

                this.Put(this.theA);
                this.theA = this.Get();
            }
        }

        private bool Action3(int d)
        {
            if (d > 3) return true;

            this.theB = this.Next();
            if (this.theB != '/' || (this.theA != '(' && this.theA != ',' && this.theA != '='
                                  && this.theA != '[' && this.theA != '!' && this.theA != ':'
                                  && this.theA != '&' && this.theA != '|' && this.theA != '?'
                                  && this.theA != '{' && this.theA != '}' && this.theA != ';' && this.theA != '\n')) return true;
            this.Put(this.theA);
            this.Put(this.theB);
            while (true)
            {
                this.theA = this.Get();
                if (this.theA == '/')
                {
                    break;
                }
                else if (this.theA == '\\')
                {
                    this.Put(this.theA);
                    this.theA = this.Get();
                }
                else if (this.theA <= '\n')
                {
                    throw new MinifierException($"Error: JSMIN unterminated Regular Expression literal : {this.theA}.\n");
                }

                this.Put(this.theA);
            }

            return false;
        }

        /// <summary>
        /// Get the next character, excluding comments.peek() is used to see 
        /// if a '/' is followed by a '/' or '*'.
        /// </summary>
        /// <returns></returns>
        private int Next()
        {
            var c = this.Get();
            if (c != '/') return c;
            switch (this.Peek())
            {
                case '/':
                {
                    return this.NextSlash();
                }
                case '*':
                {
                    return this.NextAsterisk();
                }
                default:
                {
                    return c;
                }
            }
        }

        private int NextSlash()
        {
            while (true)
            {
                var c = this.Get();
                if (c <= '\n')
                {
                    return c;
                }
            }
        }

        private int NextAsterisk()
        {
            this.Get();
            while (true)
            {
                if (this.Get() == '*')
                {
                    if (this.Peek() != '/') continue;
                    this.Get();
                    return ' ';
                }

                if (this.Get() == Eof)
                {
                    throw new MinifierException("Error: JSMIN Unterminated comment.\n");
                }
            }
        }

        /// <summary>
        /// Get the next character without getting it
        /// </summary>
        /// <returns></returns>
        private int Peek()
        {
            this.theLookahead = this.Get();
            return this.theLookahead;
        }

        /// <summary>
        /// Return the next character from stdin. Watch out for lookahead.
        /// If the character is a control character, translate it to a space or linefeed.
        /// </summary>
        /// <returns></returns>
        private int Get()
        {
            var c = this.theLookahead;
            this.theLookahead = Eof;
            if (c == Eof)
            {
                c = this.tr.Read();
            }
            if (c >= ' ' || c == '\n' || c == Eof)
            {
                return c;
            }
            return c == '\r' ? '\n' : ' ';
        }

        private void Put(int c)
        {
            this.sb.Append((char)c);
        }

        /// <summary>
        /// return true if the character is a letter, digit, underscore, 
        /// dollar sign, or non-ASCII character.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsAlphanum(int c)
        {
            return ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') ||
                    (c >= 'A' && c <= 'Z') || c == '_' || c == '$' || c == '\\' ||
                    c > 126);
        }
    }
}