using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ru.Mail.AlexBasic.BlockChain.App
{
    public class BlockChain
    {
        private IList<Block> _blocks;
        private byte[] _currentTransaction;

        /// <summary>
        /// Create chain w one block genesis
        /// </summary>
        public BlockChain() 
        {
            _blocks = new List<Block>();
            _currentTransaction = new byte[0];

            var genesisBlockProof = 10;
            var genesisBlockHash = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

            //add genesis block
            NewBlock(genesisBlockHash, genesisBlockProof);
        }

        public Block NewBlock(string previousBlockHash, int proof)
        {
            var block = new Block 
            {
                Index = _blocks.Count,
                TimeStamp = DateTimeOffset.Now,
                Data = _currentTransaction,
                Proof = proof,
                PreviousBlockHash = previousBlockHash
            };

            _currentTransaction = new byte[0];

            _blocks.Add(block);

            return block;
        }

        public int NewTransaction(byte[] data)
        {
            _currentTransaction = data;
            return _blocks.Last().Index + 1;
        }

        public Block LastBlock()
        {
            return _blocks.Last();
        }

        public static int ProofOfWork(int lastProof) 
        {
            var proof = 0;
            while (!IsValidProof(lastProof, proof))
            {
                proof++;
            }
            return proof;
        }

        public static bool IsValidProof(int lastProof, int proof) 
        {
            var guess = new List<byte>();
            guess.AddRange(BitConverter.GetBytes(lastProof).Reverse());
            guess.AddRange(BitConverter.GetBytes(proof).Reverse());
            var guess_hash = new SHA256Managed().ComputeHash(guess.ToArray());

            var difficulty = 16;
            //bytes is zero
            //var isZero = false;
            //for (var i = 0; i < difficulty; i++)
            //{
            //    isZero &= (guess_hash[i] == 0);
            //}
            //return isZero;
            //var r = BitConverter.ToInt64(guess_hash, guess_hash.Length-8);
            //var r = BitConverter.ToInt64(guess_hash, guess_hash.Length - 8);
            long lastBytesOfHash = guess_hash[24];
            lastBytesOfHash = (lastBytesOfHash << 8) + guess_hash[25];
            lastBytesOfHash = (lastBytesOfHash << 8) + guess_hash[26];
            lastBytesOfHash = (lastBytesOfHash << 8) + guess_hash[27];
            lastBytesOfHash = (lastBytesOfHash << 8) + guess_hash[28];
            lastBytesOfHash = (lastBytesOfHash << 8) + guess_hash[29];
            lastBytesOfHash = (lastBytesOfHash << 8) + guess_hash[30];
            lastBytesOfHash = (lastBytesOfHash << 8) + guess_hash[31];

            return lastBytesOfHash % (1 << difficulty) == 0;
        }

        public static string Hash(Block block)
        {
            var serializedBlock = Serialize(block);

            var hashAlgorithm = new SHA256Managed();
            var hash = hashAlgorithm.ComputeHash(serializedBlock);

            string result = string.Empty;
            foreach (byte theByte in hash)
            {
                result += theByte.ToString("x2");
            }
            return result;
        }

        public static byte[] Serialize(Block block) 
        {
            var serializedBlock = new List<byte>();
            serializedBlock.AddRange(BitConverter.GetBytes(block.Index).Reverse());
            serializedBlock.AddRange(Encoding.UTF8.GetBytes(block.PreviousBlockHash));
            serializedBlock.AddRange(Encoding.UTF8.GetBytes(block.TimeStamp.ToString()));
            serializedBlock.AddRange(BitConverter.GetBytes(block.Proof).Reverse());
            serializedBlock.AddRange(block.Data);
            return serializedBlock.ToArray();
        }
    }
}
