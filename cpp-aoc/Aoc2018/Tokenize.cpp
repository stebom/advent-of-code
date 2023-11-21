#include <string>
#include <vector>

using namespace std;

vector<string> tokenize(const string& str, const string& delim)
{
	size_t tokenStart = 0;
	size_t delimPos = str.find_first_of(delim);
	vector<string> tokens;

	while (delimPos != string::npos)
	{
		const string tok = str.substr(tokenStart, delimPos - tokenStart);
		if (tok.size() > 0) tokens.push_back(tok);
		delimPos++;
		tokenStart = delimPos;
		delimPos = str.find_first_of(delim, delimPos);

		if (delimPos == string::npos)
		{
			const string tok = str.substr(tokenStart, delimPos - tokenStart);
			if (tok.size() > 0) tokens.push_back(tok);
		}
	}

	return tokens;
}
