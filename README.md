# ProjetaARQ 🏗️

**Um plugin para Autodesk Revit desenvolvido para otimizar fluxos de trabalho em arquitetura, com foco em organização, eficiência e boas práticas de desenvolvimento.**

---

## 📋 Visão Geral
O ProjetaARQ é um plugin modular para Revit que visa automatizar tarefas repetitivas, melhorar a precisão de projetos e oferecer ferramentas especializadas para arquitetos. Sua arquitetura foi pensada para ser **escalável**, **testável** e **fácil de manter**, seguindo padrões de código limpo e separação de responsabilidades.

---

## ✨ Funcionalidades Principais
- **ShowRoom de Familias** (Paredes, Pisos, Telhados).
- **Exportação** de Memorial.

---

## 🗂️ Estrutura do Projeto
O projeto segue uma arquitetura modular, dividida em camadas claramente definidas:

```plaintext
ProjetaARQ/
├── Core/               → Lógica independente do Revit
│   ├── Models/         → Modelos de dados (ex: Projeto, Parede)
│   ├── Services/       → Serviços de domínio (cálculos, regras)
│   └── Extensions/     → Métodos de extensão (ex: ListExtensions)
│
├── Revit/              → Integração com a API do Revit
│   ├── Services/       → Serviços (DocumentService, SelectionService)
│   └── Utils/          → Utilitários (Geometria, Transações)
│
├── Features/           → Funcionalidades (1 pasta por comando/ferramenta)
│   ├── CriarParedes/
│   │   ├── Commands/   → Classes IExternalCommand
│   │   ├── ViewModels/ → Lógica da UI (MVVM)
│   │   ├── Views/      → Janelas WPF (XAML)
│   │   └── Services/   → Serviços específicos da feature
│   └── ExportarExcel/  → Exemplo de outra funcionalidade
│
├── Common/             → Recursos compartilhados
│   ├── Icons/          → Ícones (.png, .ico)
│   ├── Themes/         → Estilos WPF (Styles.xaml)
│   └── Constants.cs    → Constantes globais
│
└── Tests/              → Testes unitários