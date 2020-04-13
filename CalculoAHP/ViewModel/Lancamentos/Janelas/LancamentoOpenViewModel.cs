using CalculoAHP.Model.Dominio.Relatorios;
using CalculoAHP.Model.Lancamentos.Janelas;
using CalculoAHP.ViewModel.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CalculoAHP.ViewModel.Lancamentos.Janelas
{
    public class LancamentoOpenViewModel : ViewModelBase
    {
        public LancamentoOpenViewModel(Lancamento lancamentoView)
        {
            Model = new LancamentoOpenModel(lancamentoView);
        }

        public LancamentoOpenModel Model
        {
            get;
            set;
        }

        private ICommand m_return;

        public ICommand Return
        {
            get { return m_return ?? (m_return = new RelayCommand(Retornar)); }
        }

        private ICommand m_gerarRelatorio;

        public ICommand Gerar
        {
            get { return m_gerarRelatorio ?? (m_gerarRelatorio = new RelayCommand(GerarRelatorio)); }
        }

        private void GerarRelatorio()
        {
            Relatorio.Instance.GerarRelatorio(Model.Lancamento);
            MessageBox.Show("Relatório gerado com sucesso.");
        }

        private void Retornar()
        {
            WindowManager.Instance.CloseLancamentoView();
        }
    }
}
