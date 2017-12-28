using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ru.Mail.AlexBasic.BlockChain.App.Test
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void CreateGenesisBlock() 
        {
            var chain = new BlockChain();

            var lastBlock = chain.LastBlock();

            Assert.AreEqual(0, lastBlock.Index);
            Assert.AreEqual(10, lastBlock.Proof);
            Assert.AreEqual("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", lastBlock.PreviousBlockHash);
            Assert.AreEqual(0, lastBlock.Data.Length);
            Assert.AreNotEqual(default(DateTimeOffset), lastBlock.TimeStamp);
        }

        [Test]
        public void ProofOfWorkTest() 
        {
            var lastProof = 6432;
            
            var proof = BlockChain.ProofOfWork(lastProof);


            //verify
            var guess = new List<byte>();
            guess.AddRange(BitConverter.GetBytes(lastProof).Reverse());
            guess.AddRange(BitConverter.GetBytes(proof).Reverse());

            var guess_hash = new SHA256Managed().ComputeHash(guess.ToArray());

            //Assert.AreNotEqual(0, guess_hash[27]);
            //Assert.AreNotEqual(0, guess_hash[28]);
            //Assert.AreNotEqual(0, guess_hash[29]);
            Assert.AreEqual(0, guess_hash[30]);
            Assert.AreEqual(0, guess_hash[31]);
        }

        [Test]
        public void CreateFirstBlockAfterGenesis() 
        {
            var chain = new BlockChain();

            var genesisBlock = chain.LastBlock();

            Assert.AreEqual(0, genesisBlock.Index);
            Assert.AreEqual(
                "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", 
                genesisBlock.PreviousBlockHash);

            var blockData = Encoding.UTF8.GetBytes("Hello world!");

            var index = chain.NewTransaction(blockData);

            var firstAfterGenesis = chain.LastBlock();

            Assert.AreEqual(1, firstAfterGenesis.Index);
            Assert.AreEqual(blockData, firstAfterGenesis.Data);
        }

        [Test]
        public void MineTest() 
        {
            var chain = new BlockChain();
            
            //find next proof
            var last_block = chain.LastBlock(); 
            var last_proof = last_block.Proof;
            var proof = BlockChain.ProofOfWork(last_proof);

            //add to chain
            var block = chain.NewBlock("prev hash", proof);
        }
    }
}
