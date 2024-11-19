using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture()]
    public class CustomGuidGeneratorTests
    {
        [Test()]
        public void BasicGuidTest()
        {
            string newId = CustomGuidGenerator.GenerateGuid();
            NUnit.Framework.Assert.IsFalse(string.IsNullOrEmpty(newId));
            NUnit.Framework.Assert.AreEqual(36, newId.Length);
        }

        [Test]
        public void TestUniqueGuids()
        {
            int numberOfGuids = 1000; // Number of GUIDs to generate
            HashSet<string> generatedGuids = new HashSet<string>(); // Store generated GUIDs for uniqueness check

            for (int i = 0; i < numberOfGuids; i++)
            {
                string customGuid = CustomGuidGenerator.GenerateGuid();

                // Check if the generated GUID is already in the set
                NUnit.Framework.Assert.IsFalse(generatedGuids.Contains(customGuid), $"Duplicate GUID generated: {customGuid}");

                generatedGuids.Add(customGuid); // Add the GUID to the set
            }

            // Assert that the number of unique GUIDs matches the expected count
            NUnit.Framework.Assert.AreEqual(numberOfGuids, generatedGuids.Count);
        }

        [Test]
        public async Task TestUniqueCustomGuidsInParallel()
        {
            int numberOfGuids = 10; // Number of GUIDs to generate
            HashSet<string> generatedGuids = new HashSet<string>(); // Store generated GUIDs for uniqueness check

            var tasks = new List<Task>();

            for (int i = 0; i < numberOfGuids; i++)
            {
                Task task = Task.Run(() =>
                {
                    string customGuid = CustomGuidGenerator.GenerateGuid();

                    // Check if the generated GUID is already in the set
                    NUnit.Framework.Assert.IsFalse(generatedGuids.Contains(customGuid), $"Duplicate GUID generated: {customGuid}");

                    // Add the GUID to the set in a thread-safe manner
                    lock (generatedGuids)
                    {
                        generatedGuids.Add(customGuid);
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            // Assert that the number of unique GUIDs matches the expected count
            NUnit.Framework.Assert.AreEqual(numberOfGuids, generatedGuids.Count);
        }
    }
}