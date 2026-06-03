using DFOW = DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjetaARQ.Commands.WordExport.Services
{
    internal class WordEditor : IDisposable
    {
        private WordprocessingDocument _document;
        private MainDocumentPart _mainPart;
        private string _filePath;

        public WordEditor(string filePath)
        {
            _filePath = filePath;
            Open();
        }

        private void Open()
        {
            _document = WordprocessingDocument.Open(_filePath, true);
            _mainPart = _document.MainDocumentPart;
        }

        public void ReplaceTextInContentControl(string tag, string newText)
        {
            var sdt = _mainPart.Document.Descendants<SdtElement>()
                .FirstOrDefault(e => (e.SdtProperties.GetFirstChild<Tag>()?.Val?.Value ?? "") == tag);

            if (sdt == null) return;

            // Agora, verificamos qual é o tipo do controle que encontramos
            if (sdt is SdtBlock block)
            {
                ParagraphProperties paragraphProperties = sdt.Descendants<ParagraphProperties>().FirstOrDefault()?.CloneNode(true) as ParagraphProperties;
                RunProperties runProperties = sdt.Descendants<RunProperties>().FirstOrDefault()?.CloneNode(true) as RunProperties;

                block.SdtContentBlock.RemoveAllChildren();

                var newRun = new Run();

                if (runProperties != null)
                {
                    newRun.Append(runProperties);
                }
                else
                {
                    newRun.Append(new RunProperties());
                }

                newRun.RunProperties.Append(new DocumentFormat.OpenXml.Wordprocessing.Highlight { Val = HighlightColorValues.Green });
                newRun.Append(new Text(newText));

                var newParagraph = new Paragraph();

                if (paragraphProperties != null)
                {
                    newParagraph.Append(paragraphProperties);
                }

                newParagraph.Append(newRun);

                block.SdtContentBlock.AppendChild(newParagraph);
            }

            else if (sdt is SdtRun run)
            {
                // Se for um SdtRun (nível de texto "inline"), a lógica é mais simples
                RunProperties runProperties = run.Descendants<RunProperties>().FirstOrDefault()?.CloneNode(true) as RunProperties;

                run.SdtContentRun.RemoveAllChildren();

                var newRun = new Run();

                if (runProperties != null)
                {
                    newRun.Append(runProperties);
                }
                else
                {
                    newRun.Append(new RunProperties());
                }

                newRun.RunProperties.Append(new DocumentFormat.OpenXml.Wordprocessing.Highlight { Val = HighlightColorValues.Green });
                newRun.Append(new Text(newText));

                run.SdtContentRun.Append(newRun);
            }
        }

        //DEPRECATED
        //DEPRECATED
        //DEPRECATED
        public void ReplaceTextInsideRun(string tag, string oldWord, string newWord)
        {
            var sdt = _mainPart.Document.Descendants<SdtRun>()
                        .FirstOrDefault(e => (e.SdtProperties.GetFirstChild<Tag>()?.Val?.Value ?? "") == tag);

            if (sdt != null)
            {
                var run = sdt.Descendants<Run>().FirstOrDefault(r => r.InnerText.Contains(oldWord));
                if (run != null)
                {
                    var text = run.GetFirstChild<Text>();
                    if (text != null)
                    {
                        text.Text = text.Text.Replace(oldWord, newWord);
                        text.Parent.InsertBeforeSelf(
                            new Run(
                                new RunProperties(new Highlight { Val = HighlightColorValues.Green }),
                                new Text(newWord)
                                )
                            );

                        text.Remove(); // remove o antigo, se necessário
                    }
                }
            }
        }

        public void DeleteSectionByTag(string tag)
        {
            // Usa nosso método de busca universal para encontrar o controle
            SdtElement sdt = FindSdtElementByTag(tag);

            // Se encontrou, simplesmente remove o elemento inteiro.
            // Isso apaga o controle e tudo o que estiver dentro dele.
            sdt?.Remove();
        }

        public void DeleteParagraphByContentControlTag(string tag)
        {
            // 1. Encontra o Controle de Conteúdo (SdtElement) em qualquer parte do documento.
            // Estamos usando o mesmo método auxiliar de busca universal que criamos antes.
            SdtElement sdt = FindSdtElementByTag(tag);

            if (sdt != null)
            {
                // 2. Encontra o Parágrafo 'pai' que contém este controle.
                // O método Ancestors<> sobe na hierarquia do XML para achar o primeiro ancestral do tipo especificado.
                Paragraph paragraphToDelete = sdt.Ancestors<Paragraph>().FirstOrDefault();

                if (paragraphToDelete != null)
                {
                    // 3. Remove o parágrafo inteiro do documento.
                    paragraphToDelete.Remove();
                }
            }
        }

        private SdtElement FindSdtElementByTag(string tag)
        {
            // 1. Procura no corpo principal do documento
            SdtElement sdt = _mainPart.Document.Body.Descendants<SdtElement>()
                .FirstOrDefault(e => (e.SdtProperties.GetFirstChild<Tag>()?.Val?.Value ?? "") == tag);

            if (sdt != null) return sdt;

            // 2. Se não encontrou, procura em todos os cabeçalhos (headers)
            foreach (var headerPart in _mainPart.HeaderParts)
            {
                sdt = headerPart.Header.Descendants<SdtElement>()
                    .FirstOrDefault(e => (e.SdtProperties.GetFirstChild<Tag>()?.Val?.Value ?? "") == tag);
                if (sdt != null) return sdt;
            }

            // 3. Se ainda não encontrou, procura em todos os rodapés (footers)
            foreach (var footerPart in _mainPart.FooterParts)
            {
                sdt = footerPart.Footer.Descendants<SdtElement>()
                    .FirstOrDefault(e => (e.SdtProperties.GetFirstChild<Tag>()?.Val?.Value ?? "") == tag);
                if (sdt != null) return sdt;
            }

            // Retorna nulo se não encontrar em lugar nenhum
            return null;
        }

        public void ReplaceImage(string altText, string imageResourceName)
        {
            // 1. Encontra o elemento de Desenho (Drawing) que corresponde à imagem
            Drawing drawing = _mainPart.Document.Body.Descendants<Drawing>().FirstOrDefault(d =>
            {
                // O Texto Alternativo fica na propriedade "Description" do DocProperties
                DocProperties docProperties = d.Descendants<DocProperties>().FirstOrDefault();

                // VERSÃO CORRIGIDA:
                // Verifica de forma segura se Description e seu valor existem antes de comparar.
                return docProperties?.Description?.Value == altText;
            });

            if (drawing == null) return; // Imagem com o Alt Text não encontrada

            // 2. Encontra o Blip dentro do Desenho
            var blip = drawing.Descendants<DFOW.Blip>().FirstOrDefault();
            if (blip == null) return;

            // 3. O resto do processo é o mesmo que já fizemos:
            // Pega o ID antigo, adiciona a nova imagem, pega o novo ID e atualiza o Blip.

            string oldImagePartId = blip.Embed;

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(imageResourceName))
            {
                if (stream == null) return;

                ImagePart novaImagePart = _mainPart.AddImagePart(ImagePartType.Png); // Adapte o tipo
                novaImagePart.FeedData(stream);
                string novoIdRelacionamento = _mainPart.GetIdOfPart(novaImagePart);

                blip.Embed = novoIdRelacionamento;

                //// Apaga a parte da imagem antiga se ela não for mais usada
                //if (_mainPart.Document.Body.Descendants<DFOW.Blip>().Count(b => b.Embed == oldImagePartId) == 0)
                //{
                //    _mainPart.DeletePart(_mainPart.GetPartById(oldImagePartId));
                //}
            }
        }

        public void Dispose()
        {
            _document?.Dispose(); // Fecha e salva
        }
    }
}
