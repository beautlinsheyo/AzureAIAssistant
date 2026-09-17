
const chatForm = document.getElementById('chatForm');
const userInput = document.getElementById('userInput');
const chatWindow = document.getElementById('chatWindow');
const emptyState = document.getElementById('emptyState');
const sendBtn = document.getElementById('sendBtn');

let conversationHistory = []; // { role, content } pairs, in order

chatForm.addEventListener('submit', async (e) => {
    e.preventDefault();

    const message = userInput.value.trim();
    if (!message) return;

    if (emptyState) emptyState.remove();

    addMessage(message, 'user');
    userInput.value = '';
    sendBtn.disabled = true;

    const loadingEl = addMessage('Thinking...', 'ai loading');

    try {
        const response = await fetch('/api/Assistant', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                userMessage: message,
                history: conversationHistory
            })
        });

        if (!response.ok) {
            throw new Error('Request failed: ' + response.status);
        }

        const data = await response.json();
        loadingEl.remove();
        addMessage(data.aiResponse, 'ai');

        // Update local history AFTER a successful round-trip
        conversationHistory.push({ role: 'user', content: message });
        conversationHistory.push({ role: 'assistant', content: data.aiResponse });

    } catch (err) {
        loadingEl.remove();
        addMessage('Something went wrong. Please try again.', 'ai');
        console.error(err);
    } finally {
        sendBtn.disabled = false;
        userInput.focus();
    }
});

function addMessage(text, cssClass) {
    const div = document.createElement('div');
    div.className = 'message ' + cssClass;
    div.textContent = text;
    chatWindow.appendChild(div);
    chatWindow.scrollTop = chatWindow.scrollHeight;
    return div;
} 