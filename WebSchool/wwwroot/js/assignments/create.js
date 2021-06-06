const questionsSection = document.getElementById('questionsSection');
const questions = [];

const addNewQuestion = () => {
    const question = createQuestionHeaders();
    addAnswerField(question);
}

const createQuestionHeaders = () => {
    const questionContainer = document.createElement('div');
    questionContainer.classList.add('mb-3');

    // Question number label
    const questionNumber = document.createElement('label');
    questionNumber.classList.add('form-label', 'd-block');
    questionNumber.textContent = `#${questions.length} Question`;
    questionContainer.appendChild(questionNumber);

    // question title container
    const questionTitleContainer = document.createElement('div');
    questionTitleContainer.classList.add('mb-3');
    questionContainer.appendChild(questionTitleContainer);

    // multiple questions label
    const multipleQuestionsLabel = document.createElement('label');
    multipleQuestionsLabel.classList.add('form-label');
    multipleQuestionsLabel.textContent = 'Multiple answers:';
    questionTitleContainer.appendChild(multipleQuestionsLabel);

    // mulitple questions checkbox
    const multipleQuestionsInput = document.createElement('input');
    multipleQuestionsInput.type = 'checkbox';
    multipleQuestionsInput.value = 'false';
    multipleQuestionsInput.name = `Questions[${questions.length}].HasMultipleAnswers`;
    multipleQuestionsInput.classList.add('form-check-input', 'ms-2');
    questionTitleContainer.appendChild(multipleQuestionsInput);

    // question textarea
    const questionTextarea = document.createElement('textarea');
    questionTextarea.classList.add('form-control', 'mb-3', 'question-textarea');
    questionTextarea.placeholder = 'Question goes here';
    questionTextarea.name = `Questions[${questions.length}].Question`;
    questionContainer.appendChild(questionTextarea);

    // answers section
    const answersSection = document.createElement('section');
    questionContainer.appendChild(answersSection);

    const question = {
        element: questionContainer,
        answersSection,
        answers: 0,
        index: questions.length
    };

    // add new answer button
    const newAnswerButton = document.createElement('button');
    newAnswerButton.textContent = 'Add answer';
    newAnswerButton.classList.add('btn', 'btn-primary');
    newAnswerButton.type = 'button';
    newAnswerButton.addEventListener('click', () => addAnswerField(question));
    questionContainer.appendChild(newAnswerButton);

    const horizontalLine = document.createElement('hr');
    questionContainer.appendChild(horizontalLine);

    questions.push(question);
    questionsSection.appendChild(questionContainer);
    return question;
}

const addAnswerField = (question) => {
    const answerContainer = document.createElement('div');
    answerContainer.classList.add('mb-3');

    const answerRadioButton = document.createElement('input');
    answerRadioButton.classList.add('form-check-input', 'align-baseline', 'me-2');
    answerRadioButton.type = 'radio';
    answerRadioButton.name = `answer-${question.index}`;
    answerContainer.appendChild(answerRadioButton);

    const answerInputField = document.createElement('input');
    answerInputField.classList.add('form-control', 'd-inline-block', 'w-75');
    answerInputField.placeholder = 'Answer';
    answerInputField.name = `Questions[${question.index}].Answers[${question.answers}].Content`;
    answerContainer.appendChild(answerInputField);

    const deleteButton = document.createElement('button');
    deleteButton.type = 'button';
    deleteButton.addEventListener('click', (event) => deleteAnswer(event));
    deleteButton.classList.add('btn', 'btn-danger', 'ms-2');
    deleteButton.style.marginTop = '-5px';
    deleteButton.innerHTML = '<i class="fa fa-trash-alt"></i>';
    answerContainer.appendChild(deleteButton);

    question.answers++;
    question.answersSection.appendChild(answerContainer);
}

const createAssignment = () => {
    let questionIndex = 0;
    for (const question of questions) {
        const questionTextarea = question.element.querySelector('#questionsSection > div > textarea');
        questionTextarea.name = `Questions[${questionIndex}].Question`;

        const multipleAnswersInput = question.element.querySelector('#questionsSection > div > div > input');
        multipleAnswersInput.name = `Questions[${questionIndex}].HasMultipleAnswers`;
        multipleAnswersInput.value = multipleAnswersInput.checked;

        let answerIndex = 0;
        const answers = Array.from(question.answersSection.children);
        for (const answer of answers) {
            const valueButton = answer.children[0];
            valueButton.name = `Questions[${questionIndex}].Answers[${answerIndex}].IsCorrect`;
            valueButton.value = valueButton.checked;

            const answerInput = answer.children[1];
            answerInput.name = `Questions[${questionIndex}].Answers[${answerIndex}].Content`;
            answerIndex++;
        }

        questionIndex++;
    }
}

const deleteAnswer = (event) => {
    const answerContainer = event.currentTarget.parentNode;
    answerContainer.remove();
}