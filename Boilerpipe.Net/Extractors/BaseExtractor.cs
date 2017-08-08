namespace Boilerpipe.Net.Extractors
{
    using System;
    using System.IO;

    using Boilerpipe.Net.Document;
    using Boilerpipe.Net.Sax;

    using global::Sax.Net;

    /// <summary>
    ///     The base class of Extractors. Also provides some helper methods to quickly
    ///     retrieve the text that remained after processing.
    /// </summary>
    public abstract class BaseExtractor : IBoilerpipeExtractor
    {
        /// <summary>
        ///     Extracts text from the HTML code given as a string.
        /// </summary>
        /// <param name="html">The HTML code as a string.</param>
        /// <returns>The extracted text.</returns>
        /// <exception cref="BoilerpipeProcessingException"></exception>
        public string GetText(string html)
        {
            try
            {
                return GetText(new BoilerpipeSAXInput(new InputSource(new StringReader(html))).GetTextDocument());
            }
            catch (Exception ex)
            {
                if (ex is BoilerpipeProcessingException)
                {
                    throw;
                }
                throw new BoilerpipeProcessingException(ex.Message, ex);
            }
        }

        /// <summary>
        ///     text from the HTML code available from the given <see cref="TextReader" />.
        /// </summary>
        /// <param name="reader">The Reader containing the HTML</param>
        /// <returns>The extracted text.</returns>
        /// <exception cref="BoilerpipeProcessingException"></exception>
        public string GetText(TextReader reader)
        {
            try
            {
                return GetText(new BoilerpipeSAXInput(new InputSource(reader)).GetTextDocument());
            }
            catch (Exception ex)
            {
                if (ex is BoilerpipeProcessingException)
                {
                    throw;
                }
                throw new BoilerpipeProcessingException(ex.Message, ex);
            }
        }

        /// <summary>
        ///     Extracts text from the given <see cref="TextDocument" /> object.
        /// </summary>
        /// <param name="doc">The <see cref="TextDocument" />.</param>
        /// <returns>The extracted text.</returns>
        /// <exception cref="BoilerpipeProcessingException"></exception>
        public string GetText(TextDocument doc)
        {
            try
            {
                Process(doc);
                return doc.Content;
            }
            catch (Exception ex)
            {
                throw new BoilerpipeProcessingException(ex.Message, ex);
            }
        }

        /// <summary>
        ///     Processes the given document <code>doc</code>.
        /// </summary>
        /// <param name="doc">The <see cref="TextDocument" /> that is to be processed.</param>
        /// <returns><code>true</code> if changes have been made to the <see cref="TextDocument" />.</returns>
        /// <exception cref="BoilerpipeProcessingException"></exception>
        public abstract bool Process(TextDocument doc);

        /// <summary>
        ///     Extracts text from the HTML code available from the given <see cref="InputSource" />.
        /// </summary>
        /// <param name="inputSource">The <see cref="InputSource" /> containing the HTML.</param>
        /// <returns>The extracted text.</returns>
        /// <exception cref="BoilerpipeProcessingException"></exception>
        public string GetText(InputSource inputSource)
        {
            try
            {
                return GetText(new BoilerpipeSAXInput(inputSource).GetTextDocument());
            }
            catch (Exception ex)
            {
                throw new BoilerpipeProcessingException(ex.Message, ex);
            }
        }
    }
}