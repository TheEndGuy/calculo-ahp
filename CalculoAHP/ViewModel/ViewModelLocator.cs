using CalculoAHP.ViewModel.MainScreens;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace CalculoAHP.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<RegistroModeloMain>();
            SimpleIoc.Default.Register<ModeloRegistroViewModel>();
            SimpleIoc.Default.Register<ModeloSelecaoViewModel>();

            SimpleIoc.Default.Register<RegistroLancamentoMain>();
            SimpleIoc.Default.Register<LancamentoAlternativaViewModel>();
            SimpleIoc.Default.Register<LancamentoNotasViewModel>();
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        #region Cadastro de lançamento

        // Tela principal do registro de lançamentos
        public RegistroLancamentoMain MainRegisterLancamento
        {
            get { return ServiceLocator.Current.GetInstance<RegistroLancamentoMain>(); }
        }

        // Primeira etapa registro lançamentos (alternativas)
        public LancamentoAlternativaViewModel RegistroAlternativasLancamento
        {
            get { return ServiceLocator.Current.GetInstance<LancamentoAlternativaViewModel>(); }
        }

        // Segunda etapa registro lançamentos (notas)
        public LancamentoNotasViewModel RegistroNotasLancamento
        {
            get { return ServiceLocator.Current.GetInstance<LancamentoNotasViewModel>(); }
        }
        
        #endregion

        #region Cadastro de modelo

        // Tela principal do registro de modelos
        public RegistroModeloMain MainRegisterModel
        {
            get { return ServiceLocator.Current.GetInstance<RegistroModeloMain>(); }
        }

        // Primeira etapa registro modelos (registro dos critérios)
        public ModeloRegistroViewModel RegistroCriteriosModelo
        {
            get { return ServiceLocator.Current.GetInstance<ModeloRegistroViewModel>(); }
        }

        // Segunda etapa registro modelos (seleção das prioridades)
        public ModeloSelecaoViewModel RegistroSelecaoModelo
        {
            get { return ServiceLocator.Current.GetInstance<ModeloSelecaoViewModel>(); }
        }

        #endregion

        public static void Cleanup()
        {
        }
    }
}