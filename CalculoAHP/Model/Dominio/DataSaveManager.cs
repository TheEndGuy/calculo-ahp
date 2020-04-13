using CalculoAHP.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CalculoAHP.Model.Dominio
{
    public class DataSaveManager
    {
        private readonly string BaseDirectoryLancamento = System.AppDomain.CurrentDomain.BaseDirectory + @"\Lancamentos";

        private readonly string BaseDirectoryModelo = System.AppDomain.CurrentDomain.BaseDirectory + @"\Modelos";
        
        private DataSaveManager()
        {
            Modelos = new List<Modelo>();
            Lancamentos = new List<Lancamento>();
            Serializer = new JsonSerializer();

            LoadLancamentos();
            LoadModelos();

            LancamentosModelos();
        }

        private static DataSaveManager m_saveManager;

        public static DataSaveManager Instance
        {
            get { return m_saveManager ?? (m_saveManager = new DataSaveManager()); }
            set { m_saveManager = value; }
        }

        private JsonSerializer Serializer
        {
            get;
            set;
        }

        private List<Modelo> Modelos
        {
            get;
            set;
        }

        private List<Lancamento> Lancamentos
        {
            get;
            set;
        }
        
        #region Gets

        public int GetModeloNextId()
        {
            if (Modelos.Count == 0)
                return 1;

            return Modelos.OrderBy(entry => entry.Id).Last().Id + 1;
        }

        public int GetLancamentoNextId()
        {
            if (Lancamentos.Count == 0)
                return 1;

            return Lancamentos.OrderBy(entry => entry.Id).Last().Id + 1;
        }

        public List<Modelo> GetModelos()
        {
            return Modelos;
        }
        
        public List<Lancamento> GetLancamentosModelo(Modelo modeloLancamento)
        {
            return Lancamentos.Where(entry => entry.Modelo.Id == modeloLancamento.Id).ToList();
        }

        #endregion

        #region Create

        public void SalvarModelo(Modelo modeloSave)
        {
            string m_Directory = BaseDirectoryModelo + @"\" + modeloSave.Id;
            Directory.CreateDirectory(m_Directory);

            using (StreamWriter sw = new StreamWriter(m_Directory + @"\modelo_" + modeloSave.Id + ".json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                Serializer.Serialize(writer, modeloSave);
            }

            if (Modelos.Any(entry => entry.Id == modeloSave.Id))
            {
                var oldModelo = Modelos.FirstOrDefault(entry => entry.Id == modeloSave.Id);
                Modelos.Remove(oldModelo);
            }

            Modelos.Add(modeloSave);
        }

        public void SalvarLancamento(Lancamento lancamentoSave)
        {
            string m_Directory = BaseDirectoryLancamento + @"\" + lancamentoSave.Id;
            Directory.CreateDirectory(m_Directory);

            using (StreamWriter sw = new StreamWriter(m_Directory + @"\lancamento_" + lancamentoSave.Id + ".json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                Serializer.Serialize(writer, lancamentoSave);
            }

            if(Lancamentos.Any(entry=> entry.Id == lancamentoSave.Id))
            {
                var oldLancamento = Lancamentos.FirstOrDefault(entry => entry.Id == lancamentoSave.Id);
                Lancamentos.Remove(oldLancamento);
            }

            Lancamentos.Add(lancamentoSave);
        }

        #endregion

        #region Read

        private void LoadLancamentos()
        {
            Directory.CreateDirectory(BaseDirectoryLancamento);

            foreach (var file in Directory.GetFiles(BaseDirectoryLancamento, "*", SearchOption.AllDirectories))
            {
                using (StreamReader streamFile = File.OpenText(file))
                {
                    Lancamento lancamentoItem = (Lancamento)Serializer.Deserialize(streamFile, typeof(Lancamento));

                    if (lancamentoItem != null)
                        Lancamentos.Add(lancamentoItem);
                }
            }
        }

        private void LoadModelos()
        {
            Directory.CreateDirectory(BaseDirectoryModelo);

            foreach (var file in Directory.GetFiles(BaseDirectoryModelo, "*", SearchOption.AllDirectories))
            {
                using (StreamReader streamFile = File.OpenText(file))
                {
                    Modelo modeloItem = (Modelo)Serializer.Deserialize(streamFile, typeof(Modelo));

                    if (modeloItem != null)
                        Modelos.Add(modeloItem);
                }
            } 
        }

        /// <summary>
        /// Obtém todos os modelos de lançamentos que não estão cadastrados
        /// </summary>
        private void LancamentosModelos()
        {
            var lancamentosModelo = Lancamentos.Where(entry => !Modelos.Any(x => x.Id == entry.Modelo.Id)).GroupBy(x => x.Modelo.Id).ToList();

            if (lancamentosModelo.Count == 0)
                return;

            MessageBoxResult result = MessageBox.Show("Você possui " + lancamentosModelo.Count + " modelos sem cadastro.\n\rDeseja cadastrá-los ?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;

            foreach (var lancamentoModelo in lancamentosModelo)
                SalvarModelo(Lancamentos.FirstOrDefault(entry => entry.Modelo.Id == lancamentoModelo.Key).Modelo);
        }

        #endregion

        #region Update

        public void AtualizarLancamento(Lancamento lancamentoAtualizacao)
        {
            var lancamento = Lancamentos.FirstOrDefault(entry => entry.Id == lancamentoAtualizacao.Id);

            if (lancamento == null)
                return;

            lancamento.AssignFields(lancamentoAtualizacao);
            SalvarLancamento(lancamento);
        }

        public void AtualizarModelo(Modelo modeloAtualizacao)
        {
            var modelo = Modelos.FirstOrDefault(entry => entry.Id == modeloAtualizacao.Id);

            if (modelo == null)
                return;

            modelo.AssignFields(modeloAtualizacao);
            SalvarModelo(modelo);
        }

        #endregion

        #region Delete

        public void RemoverLancamento(Lancamento lancamentoRemocao)
        {
            var lanc = Lancamentos.FirstOrDefault(entry => entry.Id == lancamentoRemocao.Id);

            if (lanc == null)
                return;

            Lancamentos.Remove(lanc);

            var lancamentoDirectory = Directory.GetDirectories(BaseDirectoryLancamento).FirstOrDefault(entry=> entry.Equals(BaseDirectoryLancamento + @"\" + lancamentoRemocao.Id));

            if (lancamentoDirectory == null || lancamentoDirectory.Length == 0)
                return;

            Directory.Delete(lancamentoDirectory, true);
        }

        public void RemoverModelo(Modelo modeloRemocao)
        {
            var model = Modelos.FirstOrDefault(entry => entry.Id == modeloRemocao.Id);

            if (model == null)
                return;

            Modelos.Remove(model);

            var modeloDirectory = Directory.GetDirectories(BaseDirectoryModelo).FirstOrDefault(entry => entry.Equals(BaseDirectoryModelo + @"\" + modeloRemocao.Id));

            if (modeloDirectory == null || modeloDirectory.Length == 0)
                return;

            Directory.Delete(modeloDirectory, true);
            RemoveLancamentosModelo(modeloRemocao.Id);
        }

        private void RemoveLancamentosModelo(int modeloId)
        {
            foreach (var lancamento in Lancamentos.Where(entry => entry.Modelo.Id == modeloId).ToList())
                RemoverLancamento(lancamento);
        }

        #endregion
    }
}
