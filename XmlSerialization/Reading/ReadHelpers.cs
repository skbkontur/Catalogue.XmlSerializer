using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Threading;

using SKBKontur.Catalogue.XmlSerialization.Reading.ContentReaders;

namespace SKBKontur.Catalogue.XmlSerialization.Reading
{
    public class ReadHelpers
    {
        static ReadHelpers()
        {
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName("ReportReader" + Guid.NewGuid()),
                AssemblyBuilderAccess.Run);
            moduleBuilder = assemblyBuilder.DefineDynamicModule("ReportReaderModule");
        }

        public static Func<T> EmitConstruction<T>()
        {
            var type = typeof(T);
            var constructorInfo = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                null, Type.EmptyTypes, null);
            if (constructorInfo == null)
                return () => (T)FormatterServices.GetUninitializedObject(type);
            var dynamicMethod = new DynamicMethod(
                Guid.NewGuid().ToString(),
                typeof(T),
                new Type[] {},
                typeof(T).Module, true);
            var il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Newobj, constructorInfo);
            il.Emit(OpCodes.Ret);
            return (Func<T>)dynamicMethod.CreateDelegate(typeof(Func<T>));
        }

        //TODO не работает если T - невидим из своей сборки
        public static IContentPropertySetter<T> BuildSetter<T>(PropertyInfo propertyInfo,
                                                               IContentReaderCollection contentWriterCollection)
        {
            lock (lockObject)
            {
                var typeBuilder =
                    moduleBuilder.DefineType(
                        typeof(T).Name + "." + propertyInfo.Name + "_" + Interlocked.Increment(ref count),
                        TypeAttributes.Public | TypeAttributes.Class, null,
                        new[] {typeof(IContentPropertySetter<T>)});

                var concreteReader = typeof(IContentReader<>).MakeGenericType(propertyInfo.PropertyType);
                var readMethod = concreteReader.GetMethod("Read");
                var field = typeBuilder.DefineField("contentReader", concreteReader, FieldAttributes.Private);
                var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public,
                                                                       CallingConventions.Standard,
                                                                       new[]
                                                                           {
                                                                               typeof(
                                                                           IContentReaderCollection
                                                                           )
                                                                           });

                var methodInfo = typeof(IContentReaderCollection).GetMethod("Get");
                var getMethod = methodInfo.MakeGenericMethod(propertyInfo.PropertyType);
                var ilGenerator = constructorBuilder.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_0); //this
                ilGenerator.Emit(OpCodes.Ldarg_1); //collection
                ilGenerator.Emit(OpCodes.Callvirt, getMethod);
                ilGenerator.Emit(OpCodes.Stfld, field);
                ilGenerator.Emit(OpCodes.Ret);

                var setMethod = propertyInfo.GetSetMethod();
                if (setMethod == null)
                    throw new MissingMemberException(propertyInfo.ReflectedType.FullName, propertyInfo.Name + ".set()");

                var methodBuilder = typeBuilder.DefineMethod("SetProperty",
                                                             MethodAttributes.Public |
                                                             MethodAttributes.Virtual |
                                                             MethodAttributes.NewSlot,
                                                             typeof(void),
                                                             new[] {typeof(T), typeof(IReader)});
                ilGenerator = methodBuilder.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_1); //target
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, field);
                ilGenerator.Emit(OpCodes.Ldarg_2);
                ilGenerator.Emit(OpCodes.Callvirt, readMethod);
                ilGenerator.Emit(OpCodes.Callvirt, setMethod);
                ilGenerator.Emit(OpCodes.Ret);

                var type = typeBuilder.CreateType();
                var publicConstructor = type.GetPublicConstructor(typeof(IContentReaderCollection));
                var result = publicConstructor.Invoke(new[] {contentWriterCollection});
                return (IContentPropertySetter<T>)result;
            }
        }

        private static readonly ModuleBuilder moduleBuilder;
        private static int count;
        private static readonly object lockObject = new object();
    }
}