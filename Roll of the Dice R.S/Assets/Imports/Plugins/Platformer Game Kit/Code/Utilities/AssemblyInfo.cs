// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

#if ! DOCS
[assembly: AssemblyTitle(nameof(PlatformerGameKit))]
[assembly: AssemblyDescription("An extensible 2D platformer character controller and general game kit based on Animancer.")]
[assembly: AssemblyProduct(PlatformerGameKit.Strings.ProductName)]
[assembly: AssemblyCompany("Kybernetik")]
[assembly: AssemblyCopyright("Copyright © Kybernetik 2021")]
[assembly: AssemblyVersion("1.0.0.0")]
#endif

[assembly: SuppressMessage("Style", "IDE0016:Use 'throw' expression",
    Justification = "Not supported by older Unity versions.")]
[assembly: SuppressMessage("Style", "IDE0019:Use pattern matching",
    Justification = "Not supported by older Unity versions.")]
[assembly: SuppressMessage("Style", "IDE0039:Use local function",
    Justification = "Not supported by older Unity versions.")]
[assembly: SuppressMessage("Style", "IDE0044:Make field readonly",
    Justification = "Using the [SerializeField] attribute on a private field means Unity will set it from serialized data.")]
[assembly: SuppressMessage("Code Quality", "IDE0051:Remove unused private members",
    Justification = "Unity messages can be private, but the IDE will not know that Unity can still call them.")]
[assembly: SuppressMessage("Code Quality", "IDE0052:Remove unread private members",
    Justification = "Unity messages can be private and don't need to be called manually.")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter",
    Justification = "Unity messages sometimes need specific signatures, even if you don't use all the parameters.")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
    Justification = "Not supported by older Unity versions.")]
[assembly: SuppressMessage("Style", "IDE0066:Convert switch statement to expression",
    Justification = "Not supported by older Unity versions.")]
[assembly: SuppressMessage("Code Quality", "IDE0067:Dispose objects before losing scope",
    Justification = "Not always relevant.")]
[assembly: SuppressMessage("Code Quality", "IDE0068:Use recommended dispose pattern",
    Justification = "Not always relevant.")]
[assembly: SuppressMessage("Code Quality", "IDE0069:Disposable fields should be disposed",
    Justification = "Not always relevant.")]
[assembly: SuppressMessage("Style", "IDE0074:Use compound assignment",
    Justification = "Not supported by Unity")]
[assembly: SuppressMessage("Style", "IDE0083:Use pattern matching",
    Justification = "Not supported by older Unity versions")]
[assembly: SuppressMessage("Style", "IDE0090:Use 'new(...)'",
    Justification = "Not supported by older Unity versions.")]
[assembly: SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression",
    Justification = "Don't give code style advice in publically released code.")]
[assembly: SuppressMessage("Style", "IDE1006:Naming Styles",
    Justification = "Don't give code style advice in publically released code.")]

[assembly: SuppressMessage("Correctness", "UNT0005:Suspicious Time.deltaTime usage",
    Justification = "Time.deltaTime is not suspicious in FixedUpdate, it has the same value as Time.fixedDeltaTime")]

[assembly: SuppressMessage("Code Quality", "CS0649:Field is never assigned to, and will always have its default value",
    Justification = "Using the [SerializeField] attribute on a private field means Unity will set it from serialized data.")]

[assembly: SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
    Justification = "Having a field doesn't mean you are responsible for creating and destroying it.")]
[assembly: SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
    Justification = "Not all events need to care about the sender.")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
    Justification = "No need to pollute the member list of implementing types.")]
[assembly: SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "No need to pollute the member list of implementing types.")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2235:MarkAllNonSerializableFields",
    Justification = "UnityEngine.Object is serializable by Unity.")]